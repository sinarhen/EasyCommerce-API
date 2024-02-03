using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Cart : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Key] public string CustomerId { get; set; }

    // TODO: Implement CartItem entity

    // Navigation properties
    public User Customer { get; set; }
    
    public ICollection<CartProduct> Products { get; set; } = new List<CartProduct>();
    
}