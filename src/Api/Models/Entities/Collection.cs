using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class Collection : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Guid StoreId { get; set; }
    
    // Navigation properties
    [ForeignKey("StoreId")]
    public Store Store { get; set; }
    public List<Product> Products { get; set; }
    public List<Billboard> Billboards { get; set; }
}