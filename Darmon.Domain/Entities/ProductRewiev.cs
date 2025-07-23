using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

internal class ProductReview : BaseEntity
{
    public int ProductId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public int UserId { get; set; } // Foreign key
    public User User { get; set; }   // Navigation back to User
}