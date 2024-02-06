using ApartmentManagementSystem.Core.DTOs;
using ApartmentManagementSystem.Models.Entities;
using AutoMapper;

namespace ApartmentManagementSystem.Core.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, AdminLoginRequestDto>().ReverseMap();
        CreateMap<User, UserLoginRequestDto>().ReverseMap();
    }
}