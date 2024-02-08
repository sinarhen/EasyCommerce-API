using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class SellerUpgradeRequests : BaseEntity
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public bool IsApproved { get; set; }
    public string Message { get; set; }
    public DateTime? DecidedAt { get; set; }



    [ForeignKey("UserId")]
    public User User { get; set; }   
}