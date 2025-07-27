using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

public class CartItem : BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UserId { get; set; }  // Foreign key
    public User User { get; set; }    // Navigation back to User
}