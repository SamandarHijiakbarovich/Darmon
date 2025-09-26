using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;
using Darmon.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


public class PaymentTransaction : BaseEntity
{

    [ForeignKey("User")]
    public int UserId { get; set; }  // Foydalanuvchi bilan bog'liq bo'lgan ID

    [Required]
    public string? TransactionId { get; set; }  // Tizim orqali yuborilgan tranzaksiya ID

    public string? PaymentTransId { get; set; }  // Click yoki boshqa to'lov tizimi orqali kelgan tranzaksiya ID

    [Required]
    public float Amount { get; set; } // Tranzaksiya summasi

    [Required]
    public DateTime CreatedAt { get; set; }  // Tranzaksiya yaratilgan vaqt

    public DateTime? LastUpdatedAt { get; set; } // Tranzaksiya oxirgi marta yangilangan vaqt

    [Required]
    public TransactionStatus Status { get; set; }  // Tranzaksiya holati (Pending, Completed, Failed, va hokazo)

    [Required]
    public PaymentType PaymentType { get; set; }  // To'lov turi (Click, Payme, UPay, va h.k.)

    // Navigatsiya xususiyati
    public User? User { get; set; }  // Tranzaksiyani amalga oshirgan foydalanuvchi

    public int? OrderId { get; set; }
    public Order? Order { get; set; }
}


