using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("CategoryId", "SizeId")]
public class CategorySize
{
    public Guid CategoryId { get; set; }
    
    public Guid SizeId { get; set; }
    
    // Navigation properties
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    [ForeignKey("SizeId")]
    public Size Size { get; set; }
}