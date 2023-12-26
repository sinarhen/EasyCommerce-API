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
    }
}