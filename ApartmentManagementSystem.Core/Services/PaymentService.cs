using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Infrastructure.Repositories;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApartmentManagementSystem.Core.Services;

public class PaymentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : IPaymentService
{
    public async Task<ResponseDto<List<PaymentGetAllResponseDto>>> GetAllPayments()
    {
        var payments = await unitOfWork.PaymentRepository.GetAllAsync();

        var paymentDtoList = mapper.Map<List<PaymentGetAllResponseDto>>(payments);
        return ResponseDto<List<PaymentGetAllResponseDto>>.Success(paymentDtoList);
    }

    public async Task<ResponseDto<List<PaymentGetByApartmentIdResponseDto>>> GetPaymentsByApartmentId(int apartmentId)
    {
        var payments = await unitOfWork.PaymentRepository.GetByApartmentIdAsync(apartmentId);

        var paymentDtoList = mapper.Map<List<PaymentGetByApartmentIdResponseDto>>(payments);
        return ResponseDto<List<PaymentGetByApartmentIdResponseDto>>.Success(paymentDtoList);
    }

    // düzenli ödeme yapan ya da yapmayan kullanıcıları getir
    // todo: kontrol et
    public async Task<ResponseDto<List<User>>> GetRegularPaymentUsers(PaymentRegularRequestDto request)
    {
        if (request.Month is < 1 or > 12)
        {
            return ResponseDto<List<User>>.Fail("Month must be between 1 and 12.");
        }

        var userIds = await unitOfWork.PaymentRepository.GetUserIdsWithRegularPayments(request.Month, request.Year);

        var users = new List<User>();

        foreach (var userId in userIds)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                users.Add(user);
            }
        }

        return ResponseDto<List<User>>.Success(users ?? []);

    }

 

    // GetLastPaymentByApartmentId
    // kullanıcı için ödeme sistemi
    // fatura/Aidat ay sonuna kadar ödenmemişse %10 ceza uygula
    // 1 sene boyunca aidatlarını düzenli ödeyen kullanıcılar, bir sonraki sene aidatlarını %10 indirimli öder
}