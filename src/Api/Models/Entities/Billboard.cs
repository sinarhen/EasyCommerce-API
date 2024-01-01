namespace ECommerce.Models.Entities;

public class Billboard
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Link { get; set; }
    public string ImageUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Collection Collection { get; set; }
}