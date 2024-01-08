using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs;
public class StoreDto : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public string LogoUrl { get; set; }
    public string Address { get; set; }
    public string Contacts { get; set; }
    public string Email { get; set; }
    public bool IsVerified { get; set; }
    public string Owner { get; set; } // TODO: Create User Dto
    public List<CollectionDto> Collections { get; set; }
}
public class CollectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<BillboardDto> Billboards { get; set; }
    public List<ProductDto> Products { get; set; }
}


public class BillboardDto
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string ImageUrl { get; set; }
    public string FilterTitle { get; set; }
    public string FilterSubtitle { get; set; }
    public BillboardFilterDto BillboardFilter { get; set; }
}

public class BillboardFilterDto
{
    public Guid Id { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public string OrderBy { get; set; }
    public decimal? FromPrice { get; set; }
    public decimal? ToPrice { get; set; }
    public string Search { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ColorId { get; set; }
    public Guid? SizeId { get; set; }
}