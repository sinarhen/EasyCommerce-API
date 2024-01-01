using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class BillboardFilter : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public Gender? Gender { get; set; }

    public Guid? CategoryId { get; set; }

    public Season? Season { get; set; }

    public Guid? ColorId { get; set; }
    
    public string OrderBy { get; set; }
    
    public decimal? FromPrice { get; set; }
    
    public decimal? ToPrice { get; set; }
    
    public Guid? SizeId { get; set; }
    
    public string Search { get; set; }
    
    public Guid? BillboardId { get; set; }
    
    // Navigation property
    [ForeignKey("BillboardId")]
    public Billboard Billboard { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    
    [ForeignKey("ColorId")]
    public Color Color { get; set; }
    
    [ForeignKey("SizeId")]
    public Size Size { get; set; }
    
    
    
}