using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("CustomerId", "ProductId")]
public class Cart
{
    [Key] public string CustomerId { get; set; }

    [Key] public Guid ProductId { get; set; }

    // Navigation properties
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}