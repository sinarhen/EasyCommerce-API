using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities;

public class Size : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [StringLength(30, ErrorMessage = "Size name is too long. max 30 characters.")]
    public string Name { get; set; }
    public int Value { get; set; }


    // Navigation properties
    public ICollection<CategorySize> Categories { get; set; } = new List<CategorySize>();
    public ICollection<Product> Stocks { get; set; } = new List<Product>();
}