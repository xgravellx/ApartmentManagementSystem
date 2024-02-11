using ApartmentManagementSystem.Core.DTOs.ApartmentDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Enums;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Core.Services;

public class ApartmentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : IApartmentService
{
    public async Task<ResponseDto<IEnumerable<ApartmentResponseDto>>> GetAll()
    {
        var apartments = await unitOfWork.ApartmentRepository.GetAllAsync();
        if (apartments == null)
        {
            return ResponseDto<IEnumerable<ApartmentResponseDto>>.Fail("No apartments found.");

        }
        var apartmentList = mapper.Map<IEnumerable<ApartmentResponseDto>>(apartments);
        return ResponseDto<IEnumerable<ApartmentResponseDto>>.Success(apartmentList);
    }

    public async Task<ResponseDto<ApartmentResponseDto>> GetById(int apartmentId)
    {
        var apartment = await unitOfWork.ApartmentRepository.GetByIdAsync(apartmentId);
        if (apartment == null)
        {
            return ResponseDto<ApartmentResponseDto>.Fail("Apartment is not found.");
        }
        var apartmentList = mapper.Map<ApartmentResponseDto>(apartment);
        return ResponseDto<ApartmentResponseDto>.Success(apartmentList);
    }

    public async Task<ResponseDto<int?>> CreateApartment(ApartmentCreateRequestDto request)
    {
        var apartmentExists = await unitOfWork.ApartmentRepository.CheckApartmentFloorAndNumberExistAsync(request.Floor, request.Number);
        if (apartmentExists)
        {
            return ResponseDto<int?>.Fail("An apartment with the specified floor and number already exists.");
        }

        var apartment = mapper.Map<Apartment>(request);
        await unitOfWork.ApartmentRepository.AddAsync(apartment);

        return ResponseDto<int?>.Success(apartment.ApartmentId);
    }

    public async Task<ResponseDto<bool?>> UpdateApartment(ApartmentUpdateRequestDto request)
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

        var isAssigned = await unitOfWork.ApartmentRepository.IsUserIdAssignedToAnotherApartment(request.ApartmentId, request.UserId.ToString());
        if (isAssigned)
        {
            return ResponseDto<bool?>.Fail("This user is assigned to another apartment.");
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

    public async Task<ResponseDto<bool?>> AssignApartmentToUser(ApartmentAssignUserRequestDto request)
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

        var isAssigned = await unitOfWork.ApartmentRepository.IsUserAssignedToAnyApartmentAsync(request.UserId);
        if (isAssigned)
        {
            return ResponseDto<bool?>.Fail("This user is already assigned to an apartment.");
        }

        user.ApartmentId = request.ApartmentId;

        apartment.UserId = request.UserId;
        apartment.Status = true;

        await userManager.UpdateAsync(user);
        await unitOfWork.ApartmentRepository.UpdateAsync(apartment);
        return ResponseDto<bool?>.Success(true);
    }

}