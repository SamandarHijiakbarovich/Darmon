using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
using Darmon.Domain.Exceptions;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories;

public class PaymentTransactionRepository : Repository<PaymentTransaction>, IPaymentTransactionRepository
{
    private readonly AppDbContext _dbcontext;

    public PaymentTransactionRepository(AppDbContext applicationDbContext) : base(applicationDbContext)
    {
        _dbcontext = applicationDbContext;
    }

    public async Task<string> GenerateTransactionIdAsync()
    {
        try
        {
            // Bazadan eng so'nggi TransactionId ni olish
            var lastTransaction = await _dbcontext.PaymentTransactions
                .OrderByDescending(t => t.CreatedAt) // Yangi transactionni olish uchun so'ngi tarixni tartibga solish
                .FirstOrDefaultAsync();
            // Agar bazada avvalgi transaction bo'lmasa, boshlang'ich qiymatni belgilash
            string lastTransactionId = lastTransaction?.TransactionId ?? "INV-A000000";

            // Transaction ID ni ajratish
            string[] parts = lastTransactionId.Split('-');
            string prefix = parts[0];  // INV
            char letterPart = parts[1][0];  // A
            int numberPart = int.Parse(parts[1].Substring(1));  // 000001

            // Agar raqam 999999 bo'lsa, harfni oshir
            if (numberPart == 999999)
            {
                // Harfni oshirish
                letterPart = (char)(letterPart + 1);
                numberPart = 1;  // Raqamni qayta boshlash
            }
            else
            {
                numberPart++;  // Raqamni oshirish
            }

            // Yangi transaction ID ni yaratish
            string transactionId = $"{prefix}-{letterPart}{numberPart:000000}";  // Raqamni 6 xonali qilib ko'rsatish

            return await Task.FromResult(transactionId);
        }
        catch
        {
            throw new Exception("TransactionId ni yaratishda xatolik");
        }
    }

    // Transactionni ID bo'yicha olish
    public async Task<PaymentTransaction> GetTransactionByTransactionIdAsync(string transactionId)
    {
        try
        {
            return await _dbcontext.PaymentTransactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task SaveChangesAsync(PaymentTransaction transaction)
    {
        _dbcontext.PaymentTransactions.Add(transaction);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task UpdateChangesAsync(PaymentTransaction transaction)
    {
        _dbcontext.PaymentTransactions.Update(transaction);
        await _dbcontext.SaveChangesAsync();
    }


}