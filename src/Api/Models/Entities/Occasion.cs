using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Occasion : BaseEntity
{
    [Key] public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }


    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}