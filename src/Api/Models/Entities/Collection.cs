using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class Collection : BaseEntity
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name should be between 6 and 100 characters.")]
    public string Name { get; set; }
    
    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }

    public Guid StoreId { get; set; }

    // Navigation properties
    [ForeignKey("StoreId")] public Store Store { get; set; }

    [Required] public List<Product> Products { get; set; }

    [Required] public List<Billboard> Billboards { get; set; }
}