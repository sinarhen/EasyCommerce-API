using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class BannedStore : BaseEntity
{
    [Key] public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    [Required]
    [StringLength(3000, MinimumLength = 10, ErrorMessage = "Reason message should be between 10 and 3000 characters.")]
    public string Reason { get; set; }

    public DateTime BanStartDate { get; set; }

    public DateTime? BanEndDate { get; set; }

    // Navigation property
    [ForeignKey("StoreId")] public Store Store { get; set; }
}