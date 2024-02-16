using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Material;

public class MaterialDto
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string Name { get; set; }
    
    public double Percentage { get; set; }
}