using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Common;

internal class Notification : BaseEntity
{
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public int UserId { get; set; }  // Foreign key
    public User User { get; set; }    // Navigation back to User
}