using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Color;

public class ColorDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name should be between 6 and 100 characters.")]
    public string Name { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Hex code must be 6 characters long.(e.g. FFFFFF)")]
    public string HexCode { get; set; }

    public List<string> ImageUrls { get; set; }
}