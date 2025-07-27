using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

public class ProductReview : BaseEntity
{
    public int ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public int UserId { get; set; } // Foreign key
    public User User { get; set; }   // Navigation back to User

    // Product bilan bog'lanish
    public Product Product { get; set; }  // <- Yangi qo'shildi

}