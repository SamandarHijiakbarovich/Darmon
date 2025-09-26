using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.ClickDtos.ClickResponseDto;
using Darmon.Application.DTOs.ClickDtos.RequestResponseDto;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class ClickPaymentService : IClickPaymentService
{
    private readonly ClickSettings _clickSettings;
    private readonly IUserService _userService;
    private readonly IPaymentTransactionRepository _transactionRepository;
    private readonly ISellerWalletRepository _sellerWalletRepository; // New dependency

    public ClickPaymentService(
        IOptions<ClickSettings> clickSettings,
        IUserService userService,
        IPaymentTransactionRepository transactionRepository,
        ISellerWalletRepository sellerWalletRepository) // Inject the wallet repository
    {
        _clickSettings = clickSettings.Value;
        _userService = userService;
        _transactionRepository = transactionRepository;
        _sellerWalletRepository = sellerWalletRepository;
    }

    // `Create` method is unchanged from the previous corrected version
    public async Task<RequestResponseDto<string>> Create(float amount, int userId)
    {
        string transactionId = await _transactionRepository.GenerateTransactionIdAsync(); // Use the correct method name

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return new RequestResponseDto<string>
            {
                Success = false,
                Message = "Foydalanuvchi topilmadi",
                Data = null
            };
        }

        var url = $"{_clickSettings.Url}?service_id={_clickSettings.Service_ID}&merchant_id={_clickSettings.Merchant_ID}&amount={amount}&transaction_param={transactionId}&return_url={_clickSettings.ReturnUrl}";

        var transaction = new PaymentTransaction
        {
            UserId = userId,
            TransactionId = transactionId,
            Amount = amount,
            CreatedAt = DateTime.UtcNow,
            Status = TransactionStatus.Pending,
            PaymentType = PaymentType.Click
        };

        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        return new RequestResponseDto<string>
        {
            Success = true,
            Message = "Havola yaratildi",
            Data = url
        };
    }

    // `Prepare` method remains the same as it correctly uses the repository
    public async Task<ClickPrepareResponseDto> Prepare(ClickPrepareRequestDto clickRequest)
    {
        // ... (Logic remains the same as the corrected version in the previous response)
        string calculatedSignString = await GenerateSignString_Prepare(
            clickRequest.click_trans_id, clickRequest.service_id, clickRequest.merchant_trans_id,
            clickRequest.amount, clickRequest.action, clickRequest.sign_time);

        if (!calculatedSignString.Equals(clickRequest.sign_string, StringComparison.OrdinalIgnoreCase))
        {
            return new ClickPrepareResponseDto { error = 1, error_note = "Invalid sign_string" };
        }

        var dbTransaction = await _transactionRepository.GetTransactionByTransactionIdAsync(clickRequest.merchant_trans_id);

        if (dbTransaction == null)
        {
            return new ClickPrepareResponseDto { error = 1, error_note = "Transaction not found" };
        }

        if (dbTransaction.Status != TransactionStatus.Pending)
        {
            return new ClickPrepareResponseDto { error = -6, error_note = "Transaction already processed" };
        }

        dbTransaction.LastUpdatedAt = DateTime.UtcNow;
        dbTransaction.PaymentTransId = clickRequest.click_trans_id.ToString();

        await _transactionRepository.UpdateAsync(dbTransaction);
        await _transactionRepository.SaveChangesAsync();

        var result = new ClickPrepareResponseDto
        {
            click_trans_id = clickRequest.click_trans_id,
            merchant_trans_id = clickRequest.merchant_trans_id,
            merchant_prepare_id = dbTransaction.TransactionId,
            error = 0,
            error_note = "Success"
        };
        return result;
    }

    // Corrected `Complete` method
    public async Task<ClickCompleteResponseDto> Complete(ClickCompleteRequestDto clickRequest)
    {
        string calculatedSignString = await GenerateSignString_Complete(
            clickRequest.click_trans_id, clickRequest.service_id, clickRequest.click_paydoc_id,
            clickRequest.merchant_trans_id, clickRequest.merchant_prepare_id, clickRequest.amount,
            clickRequest.action, clickRequest.sign_time);

        if (!calculatedSignString.Equals(clickRequest.sign_string, StringComparison.OrdinalIgnoreCase))
        {
            return new ClickCompleteResponseDto { error = 1, error_note = "Invalid sign_string" };
        }

        var dbTransaction = await _transactionRepository.GetTransactionByTransactionIdAsync(clickRequest.merchant_trans_id);

        if (dbTransaction == null)
        {
            return new ClickCompleteResponseDto { error = 1, error_note = "Transaction not found" };
        }

        // Check if the transaction is already completed to prevent duplicate payments
        if (dbTransaction.Status == TransactionStatus.Completed)
        {
            return new ClickCompleteResponseDto { error = 0, error_note = "Transaction already completed" };
        }

        if (dbTransaction.Status != TransactionStatus.Pending)
        {
            return new ClickCompleteResponseDto { error = -6, error_note = "Transaction not in pending state" };
        }

        // Ensure the amount matches to prevent tampering
        if (Math.Abs(dbTransaction.Amount - clickRequest.amount) > 0.01f)
        {
            return new ClickCompleteResponseDto { error = -2, error_note = "Invalid amount" };
        }

        // This is the correct logic for updating the balance
        if (clickRequest.error == 0)
        {
            // Retrieve the seller's wallet directly from its repository
            var sellerWallet = await _sellerWalletRepository.GetByUserIdAsync(dbTransaction.UserId);

            if (sellerWallet == null)
            {
                // This scenario should be rare, but we handle it by creating a new wallet
                sellerWallet = new SellerWallet { UserId = dbTransaction.UserId, Balance = 0m };
                await _sellerWalletRepository.AddAsync(sellerWallet);
                // The SaveChangesAsync() will be called at the end
            }

            // Use the atomic update method to ensure data integrity
            await _sellerWalletRepository.UpdateBalanceAtomicAsync(sellerWallet.Id, (decimal)dbTransaction.Amount);

            dbTransaction.Status = TransactionStatus.Completed;
        }
        else
        {
            dbTransaction.Status = TransactionStatus.Failed;
        }

        dbTransaction.LastUpdatedAt = DateTime.UtcNow;

        await _transactionRepository.UpdateAsync(dbTransaction);
        await _transactionRepository.SaveChangesAsync();

        var result = new ClickCompleteResponseDto
        {
            click_trans_id = clickRequest.click_trans_id,
            merchant_trans_id = clickRequest.merchant_trans_id,
            error = clickRequest.error,
            error_note = clickRequest.error_note
        };

        return result;
    }

    // The `GenerateSignString` methods are correct and remain unchanged.
    public async Task<string> GenerateSignString_Complete(long click_trans_id, int service_id, long click_paydoc_id, string merchant_trans_id, int merchant_prepare_id, float amount, int action, string sign_time)
    {
        string secretKey = _clickSettings.Secret_Key;
        string signString = $"{click_trans_id}{service_id}{secretKey}{merchant_trans_id}{merchant_prepare_id}{amount}{action}{sign_time}";

        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(signString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            var sb = new System.Text.StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return await Task.FromResult(sb.ToString());
        }
    }

    public async Task<string> GenerateSignString_Prepare(long clickTransId, int serviceId, string merchantTransId, float amount, int action, string signTime)
    {
        string secretKey = _clickSettings.Secret_Key;
        string concatenatedString = $"{clickTransId}{serviceId}{secretKey}{merchantTransId}{amount}{action}{signTime}";

        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(concatenatedString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return await Task.FromResult(sb.ToString());
        }
    }
}