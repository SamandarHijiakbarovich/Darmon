using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.ClickDtos.ClickResponseDto;
using Darmon.Application.DTOs.PaymentDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class ClickPaymentService : IClickPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;

    public ClickPaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["Click:SecretKey"];
    }

    public async Task<ClickPrepareResponseDto> ProcessPrepareRequestAsync(ClickPrepareRequestDto request)
    {
        // Signature ni tekshirish
        if (!ValidateClickSignature(request))
        {
            return new ClickPrepareResponseDto
            {
                Error = -1,
                Error_Note = "Invalid signature"
            };
        }

        // Prepare javobi
        return new ClickPrepareResponseDto
        {
            Click_Trans_Id = request.Click_Trans_Id,
            Merchant_Trans_Id = request.Merchant_Trans_Id,
            Merchant_Prepare_Id = request.Merchant_Trans_Id, // Yoki database ID
            Error = 0,
            Error_Note = "Success"
        };
    }

    public async Task<ClickCompleteResponseDto> ProcessCompleteRequestAsync(ClickCompleteRequestDto request)
    {
        // ✅ Endi to'g'ri metod chaqirilyapti
        if (!ValidateClickSignature(request))
        {
            return new ClickCompleteResponseDto
            {
                Error = -1,
                Error_Note = "Invalid signature"
            };
        }

        // Complete javobi
        return new ClickCompleteResponseDto
        {
            Click_Trans_Id = request.Click_Trans_Id,
            Merchant_Trans_Id = request.Merchant_Trans_Id,
            Merchant_Confirm_Id = request.Merchant_Prepare_Id,
            Error = 0,
            Error_Note = "Success"
        };
    }

    private bool ValidateClickSignature(ClickCompleteRequestDto request)
    {
        throw new NotImplementedException();
    }

    public bool ValidateClickSignature(ClickPrepareRequestDto request)
    {
        var generatedSignature = GenerateClickSignature(request);
        return generatedSignature.Equals(request.Sign_String, StringComparison.OrdinalIgnoreCase);
    }

    public string GenerateClickSignature(ClickPrepareRequestDto request)
    {
        var signString = $"{request.Click_Trans_Id}" +
                         $"{request.Service_Id}" +
                         $"{_secretKey}" +
                         $"{request.Merchant_Trans_Id}" +
                         $"{request.Amount}" +
                         $"{request.Action}" +
                         $"{request.Sign_Time}";

        return ComputeMD5Hash(signString);
    }

    private string ComputeMD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }

    public Task<bool> ValidateClickSignatureAsync(ClickPrepareRequestDto request)
    {
        return Task.FromResult(ValidateClickSignature(request));
    }

    public Task<string> GenerateClickSignatureAsync(ClickPrepareRequestDto request)
    {
        return Task.FromResult(GenerateClickSignature(request));
    }

    
}