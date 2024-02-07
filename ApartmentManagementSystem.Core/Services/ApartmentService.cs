using ApartmentManagementSystem.Core.DTOs.ApartmentDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ApartmentManagementSystem.Core.Services;

public class ApartmentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : IApartmentService
{
    public async Task<ResponseDto<IEnumerable<ApartmentGetAllResponseDto>>> GetAll()
    {
        var apartments = await unitOfWork.ApartmentRepository.GetAllAsync();
        if (apartments == null)
        {
            return ResponseDto<IEnumerable<ApartmentGetAllResponseDto>>.Fail("No apartments found.");

        }
        var apartmentList = mapper.Map<IEnumerable<ApartmentGetAllResponseDto>>(apartments);
        return ResponseDto<IEnumerable<ApartmentGetAllResponseDto>>.Success(apartmentList);
    }

    public async Task<ResponseDto<Apartment>> GetById(int apartmentId)
    {
        var apartment = await unitOfWork.ApartmentRepository.GetByIdAsync(apartmentId);
        if (apartment == null)
        {
            return ResponseDto<Apartment>.Fail("Apartment is not found.");
        }
        return ResponseDto<Apartment>.Success(apartment);
    }

    public async Task<ResponseDto<int?>> CreateApartment(ApartmentCreateRequestDto request)
    {
        var apartment = mapper.Map<Apartment>(request);
        await unitOfWork.ApartmentRepository.AddAsync(apartment);
        return ResponseDto<int?>.Success(apartment.ApartmentId);
    }

    // dönüş tipi bool olmalı.
    public async Task<ResponseDto<bool?>> UpdateApartment(ApartmentUpdateRequestDto request)
    {
        var apartment = await unitOfWork.ApartmentRepository.GetByIdAsync(request.ApartmentId);
        if (apartment == null)
        {
            return ResponseDto<bool?>.Fail("Apartment is not found.");
        }
        mapper.Map(request, apartment);
        await unitOfWork.ApartmentRepository.UpdateAsync(apartment);
        return ResponseDto<bool?>.Success(true);
    }

    public async Task<ResponseDto<bool?>> DeleteApartment(int apartmentId)
    {
        var apartment = await unitOfWork.ApartmentRepository.GetByIdAsync(apartmentId);
        if (apartment == null)
        {
            return ResponseDto<bool?>.Fail("Apartment is not found.");
        }
        await unitOfWork.ApartmentRepository.DeleteAsync(apartmentId);
        return ResponseDto<bool?>.Success(true);
    }

    public async Task<ResponseDto<bool?>> AssignUserToApartment(ApartmentAssignUserToRequestDto request)
    {
        var apartment = await unitOfWork.ApartmentRepository.GetByIdAsync(request.ApartmentId);
        if (apartment == null)
        {
            return ResponseDto<bool?>.Fail("Apartment is not found.");
        }
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return ResponseDto<bool?>.Fail("User is not found.");
        }
        apartment.UserId = request.UserId;
        await unitOfWork.ApartmentRepository.UpdateAsync(apartment);
        return ResponseDto<bool?>.Success(true);
    }

}