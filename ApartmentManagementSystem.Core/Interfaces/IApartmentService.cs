using ApartmentManagementSystem.Core.DTOs.ApartmentDto;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IApartmentService
{
    Task<ResponseDto<IEnumerable<ApartmentResponseDto>>> GetAll();
    Task<ResponseDto<ApartmentResponseDto>> GetById(int apartmentId);
    Task<ResponseDto<int?>> CreateApartment(ApartmentCreateRequestDto request);
    Task<ResponseDto<bool?>> UpdateApartment(ApartmentUpdateRequestDto request);
    Task<ResponseDto<bool?>> DeleteApartment(int apartmentId);
    Task<ResponseDto<bool?>> AssignApartmentToUser(ApartmentAssignUserRequestDto request);
}