using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;

namespace Ecommerce.RequestHelpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDto, Customer>();
        CreateMap<LoginDto, Customer>();
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(dest => dest.Category.Name))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(dest => dest.Category.Id))
            .ForMember(dest => dest.OccasionName, opt => opt.MapFrom(dest => dest.Occasion.Name))
            .ForMember(dest => dest.MainMaterialName, opt => opt.MapFrom(dest => dest.MainMaterial.Name)) 
            .ForMember(dest => dest.OrdersCount, opt => opt.MapFrom(x => x.Orders.Count))
            .ForMember(dest => dest.OrdersCountLastMonth,
                opt => opt.MapFrom(
                    x => x.Orders.Count(p => p.CreatedAt > DateTime.Now - TimeSpan.FromDays(30))))
            .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(x => x.Reviews.Count))
            .ForMember(dest => dest.AvgRating, opt => 
                opt.MapFrom(x => x.Reviews.Count == 0 ? 0 : x.Reviews.Average(r => (int) r.Rating)))
            .ForMember(dest => dest.IsNew, opt => opt.MapFrom(x => x.CreatedAt > DateTime.Now - TimeSpan.FromDays(30)))
            .ForMember(dest => dest.IsOnSale, opt => opt.MapFrom(x => x.Discount != null && x.Discount > 0))
            .ForMember(dest => dest.IsBestseller, opt => opt.MapFrom(x => x.Orders.Count > 10))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(x => x.UpdatedAt))
            .ForMember(dest => dest.Stocks, opt => opt.MapFrom(dest => dest.Stocks
                .GroupBy(ps => ps.ColorId)
                .Select(g => new StockDto
                {
                    Color = new ColorDto 
                    { 
                        Id = g.First().Color.Id, 
                        Name = g.First().Color.Name, 
                        HexCode = g.First().Color.HexCode 
                    },
                    Availability = g.Select(ps => new AvailabilityDto
                    {
                        Size = ps.Size.Name, // assuming Size has a Name property
                        Quantity = ps.Stock,
                        Price = ps.Price
                    }).ToList(),
                    ImageUrls = dest.Images
                    .Where(i => i.ColorId == g.Key)
                    .SelectMany(i => i.ImageUrls)
                    .ToList()
                })))
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(dest => dest.Materials
                .Select(pm => new MaterialDto
                {
                    Id = pm.Material.Id,
                    Name = pm.Material.Name,
                    Percentage = pm.Percentage
                })))
            ;
    }
}