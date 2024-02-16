using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Billboard;

public class WriteBillboardDto
{    
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Title must be between 6 and 100 characters.")]
    public string Title { get; set; }

    [StringLength(300, ErrorMessage = "Subtitle is too long. max 300 characters.")]
    public string Subtitle { get; set; }

    [StringLength(2000, ErrorMessage = "Image URL is too long. max 2000 characters.")]
    [Url]
    public string ImageUrl { get; set; }

    public Guid CollectionId { get; set; }

    public BillboardFilterDto BillboardFilter { get; set; }
}