using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("CustomerId", "ProductId")]
public class Cart
{
    [Key] public Guid CustomerId { get; set; }

    [Key] public Guid ProductId { get; set; }

    // Navigation properties
    public Customer Customer { get; set; }
    public Product Product { get; set; }
}