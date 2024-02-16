using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class Billboard : BaseEntity
{
    [Key] public Guid Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Title must be between 6 and 100 characters.")]
    public string Title { get; set; }

    [StringLength(300, ErrorMessage = "Subtitle is too long. max 300 characters.")]
    public string Subtitle { get; set; }
    
    [StringLength(2000, ErrorMessage = "Image URL is too long. max 2000 characters.")]
    [Url]
    public string ImageUrl { get; set; }

    public Guid CollectionId { get; set; }

    public Guid BillboardFilterId { get; set; }

    // Navigation properties
    [ForeignKey("CollectionId")] public Collection Collection { get; set; }

    [Required]
    [ForeignKey("BillboardFilterId")]
    public BillboardFilter BillboardFilter { get; set; }
}