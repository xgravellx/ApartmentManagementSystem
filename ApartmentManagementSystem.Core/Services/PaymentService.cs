﻿using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Infrastructure.Repositories;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Enums;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApartmentManagementSystem.Core.Services;

public class PaymentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IInvoiceService invoiceService) : IPaymentService
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

    public async Task<ResponseDto<int>> CreatePayment(PaymentCreateRequestDto request, bool isAdmin)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return ResponseDto<int>.Fail("User not found.");
        }

        var invoice = await unitOfWork.InvoiceRepository.GetByUserIdAsync(request.UserId);
        if (invoice == null)
        {
            return ResponseDto<int>.Fail("Invoice not found.");
        }

        var apartmentId = await unitOfWork.ApartmentRepository.GetApartmentIdsByUserIdAsync(request.UserId);
        if (!isAdmin && invoice.ApartmentId != request.ApartmentId)
        {
            return ResponseDto<int>.Fail("Access denied. Users can only access their own invoices.");
        }

        if (invoice.Amount < request.Amount || invoice.Amount > request.Amount)
        {
            return ResponseDto<int>.Fail("Invalid amount.");
        }

        var today = DateTime.UtcNow.AddHours(3);

        var payment = new Payment
        {
            InvoiceId = request.InvoiceId,
            UserId = request.UserId,
            Amount = request.Amount,
            Date = today,
            Type = request.Type,
            Method = request.Method,
        };

        await unitOfWork.PaymentRepository.AddPaymentAsync(payment);

        invoice.PaymentStatus = true;
        await unitOfWork.InvoiceRepository.UpdateInvoiceAsync(invoice);

        return ResponseDto<int>.Success(payment.PaymentId);
    }

    public async Task<ResponseDto<bool>> UpdatePayment(PaymentUpdateRequestDto request)
    {
        var payment = await unitOfWork.PaymentRepository.GetByIdAsync(request.PaymentId);
        if (payment == null)
        {
            return ResponseDto<bool>.Fail("Payment not found.");
        }

        var today = DateTime.UtcNow.AddHours(3);

        payment.Amount = request.Amount;
        payment.Date = today;
        payment.Type = request.Type;
        payment.Method = request.Method;

        await unitOfWork.PaymentRepository.UpdatePaymentAsync(payment);
        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<bool>> DeletePayment(int paymentId)
    {
        var payment = await unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            return ResponseDto<bool>.Fail("Payment not found.");
        }

        await unitOfWork.PaymentRepository.DeletePaymentAsync(paymentId);

        var invoice = await unitOfWork.PaymentRepository.GetInvoiceByPaymentIdAsync(paymentId);
        if (invoice != null)
        {
            invoice.PaymentStatus = false;
            await unitOfWork.InvoiceRepository.UpdateInvoiceAsync(invoice);
        }

        return ResponseDto<bool>.Success(true);
    }


    // GetLastPaymentByApartmentId
    // kullanıcı için ödeme sistemi
    // fatura/Aidat ay sonuna kadar ödenmemişse %10 ceza uygula
    // 1 sene boyunca aidatlarını düzenli ödeyen kullanıcılar, bir sonraki sene aidatlarını %10 indirimli öder
}