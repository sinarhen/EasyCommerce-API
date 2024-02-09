using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class Billboard : BaseEntity
{
    [Key] public Guid Id { get; set; }

    public string Title { get; set; }

    public string Subtitle { get; set; }

    public string ImageUrl { get; set; }

    public Guid CollectionId { get; set; }

    public Guid BillboardFilterId { get; set; }

    // Navigation properties
    [ForeignKey("CollectionId")] public Collection Collection { get; set; }

    [Required]
    [ForeignKey("BillboardFilterId")]
    public BillboardFilter BillboardFilter { get; set; }
}