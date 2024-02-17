using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("UserId")]
public class BannedUser
{
    public string UserId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 6, ErrorMessage = "Reason should be between 6 and 200 characters.")]
    public string Reason { get; set; }

    public DateTime BanStartTime { get; set; }
    public DateTime BanEndTime { get; set; }

    // Navigation properties
    [ForeignKey("UserId")] public User User { get; set; }
}