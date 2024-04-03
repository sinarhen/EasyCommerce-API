using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;


[PrimaryKey("UserId", "ProductId")]
public class Wishlist
{
    public string UserId { get; set; }
    public Guid ProductId { get; set; }
    
    // Navigation properties
    [ForeignKey("UserId")]
    public User User { get; set; }

}