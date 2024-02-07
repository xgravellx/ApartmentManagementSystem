using ApartmentManagementSystem.Core.DTOs.ApartmentDto;
using ApartmentManagementSystem.Core.DTOs.AuthDto;
using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Models.Entities;
using AutoMapper;

namespace ApartmentManagementSystem.Core.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, AuthAdminRequestDto>().ReverseMap();
        CreateMap<User, AuthUserRequestDto>().ReverseMap();

        // Apartment için mapping konfigürasyonu
        CreateMap<Apartment, ApartmentGetAllResponseDto>().ReverseMap();
        CreateMap<Apartment, ApartmentCreateRequestDto>().ReverseMap();
        CreateMap<Apartment, ApartmentUpdateRequestDto>().ReverseMap();
        CreateMap<Apartment, ApartmentAssignUserToRequestDto>().ReverseMap();

        // Invoice için mapping konfigürasyonu
        CreateMap<Invoice, InvoiceResponseDto>().ReverseMap();
    }
}