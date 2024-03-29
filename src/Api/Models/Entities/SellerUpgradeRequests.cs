using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class SellerUpgradeRequest : BaseEntity
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public SellerUpgradeRequestStatus Status { get; set; }

    [StringLength(3000, ErrorMessage = "Message is too long. max 3000 characters.")]
    public string Message { get; set; }

    public DateTime? DecidedAt { get; set; }

    public Guid SellerInfoId { get; set; }

    [ForeignKey("SellerInfoId")] public SellerInfo SellerInfo { get; set; }

    [ForeignKey("UserId")] public User User { get; set; }
}

public enum SellerUpgradeRequestStatus
{
    Pending,
    Approved,
    Rejected
}