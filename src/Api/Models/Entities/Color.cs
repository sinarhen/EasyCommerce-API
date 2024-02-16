using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Color : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [StringLength(maximumLength: 6, MinimumLength = 6, ErrorMessage = "Hex code must be 6 characters long.(e.g. FFFFFF)")]
    public string HexCode { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name should be between 6 and 100 characters.")]
    public string Name { get; set; }

    // Navigation properties
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
    public ICollection<OrderDetail> Orders { get; set; } = new List<OrderDetail>();
}