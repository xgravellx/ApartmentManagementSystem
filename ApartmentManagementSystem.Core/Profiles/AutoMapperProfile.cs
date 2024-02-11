using ApartmentManagementSystem.Core.DTOs.ApartmentDto;
using ApartmentManagementSystem.Core.DTOs.AuthDto;
using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;

namespace ApartmentManagementSystem.Core.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        CreateMap<User, UserRegularResponseDto>();

        CreateMap<Apartment, ApartmentResponseDto>();
        CreateMap<Apartment, ApartmentCreateRequestDto>().ReverseMap();
        CreateMap<Apartment, ApartmentUpdateRequestDto>().ReverseMap();
        CreateMap<Apartment, ApartmentAssignUserRequestDto>().ReverseMap();

        CreateMap<Invoice, InvoiceResponseDto>();
        CreateMap<Invoice, InvoiceByApartmentIdRequestDto>().ReverseMap();
        CreateMap<Invoice, InvoiceFilterRequestDto>().ReverseMap();
        CreateMap<Invoice, InvoiceDebtFilterRequestDto>().ReverseMap();
        CreateMap<Invoice, InvoiceCreateGeneralRequestDto>().ReverseMap(); 
        CreateMap<Invoice, InvoiceUpdateRequestDto>().ReverseMap();

        CreateMap<Payment, PaymentGetAllResponseDto>();
        CreateMap<Payment, PaymentDto>().ReverseMap();
        CreateMap<Payment, PaymentRegularRequestDto>().ReverseMap();
    }
}