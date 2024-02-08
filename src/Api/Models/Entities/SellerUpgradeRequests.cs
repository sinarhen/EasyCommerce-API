using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class SellerUpgradeRequests : BaseEntity
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid? SellerInfoId { get; set; }
    public User User { get; set; }
    public bool IsApproved { get; set; }
    public string Message { get; set; }
    public string AdminId { get; set; }
    public User Admin { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string RejectedReason { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string RejectedBy { get; set; }
    public User RejectedByAdmin { get; set; }

    [ForeignKey("SellerInfoId")]
    public SellerInfo SellerInfo { get; set; }

    
}