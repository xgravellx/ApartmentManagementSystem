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


    public async Task UpdateRegularStatusForUsers()
    {
        //var lastYear = DateTime.UtcNow.Year - 1;

        //// Kullanıcıları ve onlara ait daireleri ve faturaları yükleyin
        //var apartmentsWithInvoices = await unitOfWork.ApartmentRepository.GetInvoicesByApartmentIdAsync(lastYear);

        //// Her daire için, kullanıcıya ait önceki yıl içindeki tüm aidat faturalarını kontrol edin
        //foreach (var apartment in apartmentsWithInvoices)
        //{
        //    var isRegularPayer = true;

        //    // Önceki yıl için her ay, ödeme durumunu kontrol edin
        //    for (int month = 1; month <= 12; month++)
        //    {
        //        var paidInvoiceForMonth = await unitOfWork.InvoiceRepository.IsPaidDuesForMonthAsync(apartment.ApartmentId, lastYear, month);

        //        if (!paidInvoiceForMonth)
        //        {
        //            isRegularPayer = false;
        //            break;
        //        }
        //    }

        //    // İlişkili kullanıcının Regular durumunu güncelleyin
        //    var user = await userManager.FindByIdAsync(apartment.UserId.ToString());
        //    if (user != null)
        //    {
        //        user.Regular = isRegularPayer;
        //        await userManager.UpdateAsync(user);
        //    }
        //}

        //await unitOfWork.SaveChangesAsync();

    }

}