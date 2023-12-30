using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "CategoryId")]
public class ProductCategory
{
    [Key]
    public Guid ProductId { get; set; }
    [Key]
    public Guid CategoryId { get; set; }
    
    public int Order { get; set; }
    
    // Navigation properties
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}