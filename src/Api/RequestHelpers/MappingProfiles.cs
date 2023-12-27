﻿using AutoMapper;
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
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.ColorQuantities.ToDictionary(x => x.Color.Name, x => x.Quantity)));
    }
}