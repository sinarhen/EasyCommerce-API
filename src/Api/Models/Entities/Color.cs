using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Color : BaseEntity
{
    [Key] public Guid Id { get; set; }

    public string HexCode { get; set; }

    public string Name { get; set; }

    // Navigation properties
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
    public ICollection<OrderDetail> Orders { get; set; } = new List<OrderDetail>();
}