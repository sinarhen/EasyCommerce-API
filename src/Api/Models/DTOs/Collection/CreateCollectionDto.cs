namespace ECommerce.Models.DTOs.Collection;

public class CreateCollectionDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid StoreId { get; set; }


}


