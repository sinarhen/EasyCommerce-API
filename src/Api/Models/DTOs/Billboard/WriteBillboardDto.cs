namespace ECommerce.Models.DTOs.Billboard;

public class WriteBillboardDto
{
    
    public string Title { get; set; }
    
    public string Subtitle { get; set; }
    
    public string ImageUrl { get; set; }
    
    public Guid? CollectionId { get; set; }
    
    public BillboardFilterDto BillboardFilter { get; set; }
    
    public string FilterTitle { get; set; }
    
    public string FilterSubtitle { get; set; }
    

}
