using AutoMapper;
using Ecommerce.DTOs;
using Ecommerce.Entities;

namespace Ecommerce.RequestHelpers;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDto, Customer>();
        CreateMap<LoginDto, Customer>();
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name));
    }
}