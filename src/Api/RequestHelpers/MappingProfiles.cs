using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Auth;
using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.DTOs.Category;
using ECommerce.Models.DTOs.Collection;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Material;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.DTOs.Store;
using ECommerce.Models.Entities;

namespace ECommerce.RequestHelpers;

public class MappingProfiles: Profile
{

    private void MapBillboardProfiles()
    {
        CreateMap<Billboard, BillboardDto>();
        CreateMap<BillboardFilter, BillboardFilterDto>();
    }
    private void MapCategoryProfiles()
    {
        CreateMap<Category, CategoryDto>();
    }
    private void MapStoreProfiles()
    {
        CreateMap<Store, StoreDto>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName));

        CreateMap<Collection, CollectionDto>()
            .ForMember(dest => dest.Billboards, opt => opt.MapFrom(src => src.Billboards));
    }
    public MappingProfiles()
    {
        MapProductProfiles();
        MapCategoryProfiles();
        MapStoreProfiles();
        MapBillboardProfiles();
    }
    private void MapProductProfiles()
    {
        CreateMap<RegisterDto, User>();
        CreateMap<LoginDto, User>();
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Collection, opt => opt.MapFrom(x => new IdNameDto
            {
                Id = x.Collection.Id,
                Name = x.Collection.Name
            }))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(dest => dest.Categories
                .Select(pc => new ProductCategoryDto
                {
                    Id = pc.Category.Id,
                    Name = pc.Category.Name,
                    Order = pc.Order
                })
                .OrderBy(pc => pc.Order)))
            .ForMember(dest => dest.Occasion, opt => opt.MapFrom(dest => new IdNameDto
            {
                Id = dest.Occasion.Id,
                Name = dest.Occasion.Name
            }))
            .ForMember(dest => dest.MainMaterialName, opt => opt.MapFrom(dest => dest.MainMaterial.Name)) 
            .ForMember(dest => dest.OrdersCount, opt => opt.MapFrom(x => x.Orders.Count))
            .ForMember(dest => dest.OrdersCountLastMonth,
                opt => opt.MapFrom(
                    x => x.Orders.Count(p => p.CreatedAt > DateTime.Now - TimeSpan.FromDays(30))))
            .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(x => x.Reviews.Count))
            .ForMember(dest => dest.AvgRating, opt => 
                opt.MapFrom(x => x.Reviews.Count == 0 ? 0 : x.Reviews.Average(r => r.Rating)))
            .ForMember(dest => dest.IsNew, opt => opt.MapFrom(x => x.CreatedAt > DateTime.Now - TimeSpan.FromDays(30)))
            .ForMember(dest => dest.IsOnSale, opt => opt.MapFrom(x => x.Discount != null && x.Discount > 0))
            .ForMember(dest => dest.IsBestseller, opt => opt.MapFrom(x => x.Orders.Count > 10))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(x => x.UpdatedAt))
            .ForMember(dest => dest.Sizes, opt => opt.MapFrom(dest => dest.Stocks
                .Select(ps => new SizeDto
                {
                    Id = ps.Size.Id,
                    Name = ps.Size.Name,
                    Value = ps.Size.Value
                })
                .OrderBy(ps => ps.Value)
            ))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(dest => dest.Stocks.Any(ps => ps.Stock > 0)))
            .ForMember(dest => dest.Colors, opt => opt.MapFrom(dest => dest.Stocks
                .GroupBy(ps => ps.ColorId)
                .Select(g => new ColorDto
                {
                    Id = g.First().Color.Id,
                    Name = g.First().Color.Name,
                    HexCode = g.First().Color.HexCode,
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
            .ForMember(dest => dest.MinPrice, 
                opt => opt.MapFrom(src => src.Stocks.Any() ? src.Stocks.Min(s => s.Price) : 0))
            .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(s => s.Discount != null && s.Discount > 0
                ? s.Stocks.Any() ? s.Stocks.Min(productStock => productStock.Price) * (1 - (decimal) s.Discount) : 0
                : 0));


        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Materials, opt => opt.Ignore())
            .ForMember(dest => dest.Stocks, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<Product, ProductDetailsDto>()
            .IncludeBase<Product, ProductDto>()
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews
                .Select(r => new ReviewDto
                {
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                })
            ))
            .ForMember(dest => dest.Colors, opt => opt.Ignore())
            .ForMember(dest => dest.Sizes, opt => opt.Ignore())
            ;
    }
}