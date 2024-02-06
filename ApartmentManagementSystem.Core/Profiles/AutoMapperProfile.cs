using ApartmentManagementSystem.Core.DTOs.AuthDto;
using ApartmentManagementSystem.Models.Entities;
using AutoMapper;

namespace ApartmentManagementSystem.Core.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, AuthAdminRequestDto>().ReverseMap();
        CreateMap<User, AuthUserRequestDto>().ReverseMap();
    }
}