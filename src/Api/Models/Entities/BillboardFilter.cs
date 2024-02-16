using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Entities.Enum;
using ECommerce.Models.Enum;

namespace ECommerce.Models.Entities;

public class BillboardFilter : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Title should be between 6 and 100 characters.")]
    public string Title { get; set; }
    
    [StringLength(300, ErrorMessage = "Subtitle is too long.")]
    public string Subtitle { get; set; }

    public Gender? Gender { get; set; }

    public Guid? CategoryId { get; set; }

    public Season? Season { get; set; }

    public Guid? ColorId { get; set; }
    
    public ProductsOrderBy OrderBy { get; set; }

    public decimal? FromPrice { get; set; }

    public decimal? ToPrice { get; set; }

    public Guid? SizeId { get; set; }
    
    [StringLength(100, ErrorMessage = "Search term is too long.")]
    public string Search { get; set; }

    public Guid? BillboardId { get; set; }

    // Navigation property
    public Billboard Billboard { get; set; }

    [ForeignKey("CategoryId")] public Category Category { get; set; }

    [ForeignKey("ColorId")] public Color Color { get; set; }

    [ForeignKey("SizeId")] public Size Size { get; set; }
}