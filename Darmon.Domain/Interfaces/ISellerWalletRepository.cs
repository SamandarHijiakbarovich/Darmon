using Darmon.Domain.Entities;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface ISellerWalletRepository : IRepository<SellerWallet>
{
    /**
     * Maxsus repozitoriy metodlari faqat shu entity (SellerWallet) uchun domen mantig'ini bajaradi.
     */

    /// <summary>
    /// Foydalanuvchi ID (UserId) orqali SellerWallet (Hamyon) ni qidiradi.
    /// Umumiy IRepository<T> GetByIdAsync() faqat Id (WalletId) bo'yicha qidiradi.
    /// </summary>
    /// <param name="userId">Foydalanuvchining identifikatori.</param>
    /// <returns>SellerWallet obyekti yoki topilmasa null.</returns>
    Task<SellerWallet?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Balansni aniq miqdorga oshirish/kamaytirish (atomar operatsiya).
    /// Click Complete kabi tranzaksiyalarni yakunlashda foydali.
    /// </summary>
    /// <param name="walletId">Wallet identifikatori.</param>
    /// <param name="amountDelta">Balansga qo'shiladigan/ayriladigan miqdor (musbat/manfiy). Decimal tipida bo'lishi moliyaviy aniqlikni ta'minlaydi.</param>
    /// <returns>Yangilanish muvaffaqiyatli bo'lsa true.</returns>
    Task<bool> UpdateBalanceAtomicAsync(int walletId, decimal amountDelta);

    /// <summary>
    /// Balansni tekshirish uchun kerak, lekin ko'pincha GetByUserIdAsync dan keyin ishlatiladi.
    /// Bu metod asosan tranzaksiya mantiqini tekshirish uchun kerak bo'ladi (pul yechishdan oldin).
    /// </summary>
    /// <param name="walletId">Wallet identifikatori.</param>
    /// <returns>Hamyonning joriy balansini qaytaradi.</returns>
    Task<decimal> GetCurrentBalanceAsync(int walletId);
}