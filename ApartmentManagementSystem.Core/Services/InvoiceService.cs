using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Enums;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ApartmentManagementSystem.Core.Helpers;
using ApartmentManagementSystem.Infrastructure.Repositories;

namespace ApartmentManagementSystem.Core.Services;

public class InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : IInvoiceService
{
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetAll()
    {
        var invoices = await unitOfWork.InvoiceRepository.GetAllAsync();

        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices ?? []);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);
    }

    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(int apartmentId, string identityNumber, bool isAdmin)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber);
        if (user == null && !isAdmin)
        {
            return ResponseDto<List<InvoiceResponseDto>>.Fail("Apartment not found.");
        }

        var invoices = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(apartmentId);
        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);

    }

    public async Task<ResponseDto<InvoiceFilterResponseDto>> GetFiltered(InvoiceFilterRequestDto request, string identityNumber, bool isAdmin)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber);
        if (!isAdmin && user != null)
        {
            var apartmentId = await unitOfWork.ApartmentRepository.GetApartmentIdByUserIdAsync(user!.Id);
            request.ApartmentIds = [apartmentId];
            request.UserIds = [user.Id];

            if (user == null) return ResponseDto<InvoiceFilterResponseDto>.Fail("User not found");
        }

        var invoices = await unitOfWork.InvoiceRepository.GetFilteredAsync(request);
        decimal totalAmount = invoices.Sum(invoice => invoice.PayableAmount);
        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);

        var result = new InvoiceFilterResponseDto
        {
            TotalAmount = totalAmount,
            Invoices = invoiceDtoList
        };

        return ResponseDto<InvoiceFilterResponseDto>.Success(result);
    }

    public async Task<ResponseDto<decimal>> GetDebtFilter(InvoiceDebtFilterRequestDto request)
    {
        var invoices = await unitOfWork.InvoiceRepository.GetTotalDebtAsync(request.ApartmentId, request.Year, request.Month);

        if (invoices == null)
        {
            return ResponseDto<decimal>.Fail("No invoices found.");
        }

        return ResponseDto<decimal>.Success(invoices);
    }

    public async Task<ResponseDto<bool>> CreateGeneralInvoice(InvoiceCreateGeneralRequestDto request)
    {
        if (!Enum.IsDefined(typeof(InvoiceType), request.Type))
        {
            return ResponseDto<bool>.Fail("Invalid invoice type.");
        }

        if (request.Type == InvoiceType.Dues)
        {
            return ResponseDto<bool>.Fail("You can create dues for each apartment.");
        }

        var activeApartments = await unitOfWork.ApartmentRepository.GetActiveApartmentsByBlockAsync(request.Block);
        if (!activeApartments.Any())
        {
            return ResponseDto<bool>.Fail("No active apartments found for the block.");
        }

        var isValidationMonthAndYear = DateHelper.IsValidMonth(request.Month) && DateHelper.IsValidYear(request.Year);
        if (!isValidationMonthAndYear)
        {
            return ResponseDto<bool>.Fail("Invalid month or year.");
        }

        var totalApartments = activeApartments.Count;
        var amountPerApartment = request.Amount / totalApartments;

        foreach (var apartment in activeApartments)
        {
            var dueDate = DateHelper.CalculateDueDate(request.Year, request.Month);

            var invoice = new Invoice
            {
                Type = request.Type,
                Amount = amountPerApartment,
                PayableAmount = amountPerApartment,
                Year = request.Year,
                Month = request.Month,
                PaymentStatus = false,
                ApartmentId = apartment.ApartmentId,
                DueDate = dueDate
            };

            await unitOfWork.InvoiceRepository.AddInvoiceAsync(invoice);
        }

        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<bool>> CreateDuesInvoice(InvoiceCreateDuesRequestDto request)
    {
        if (request.Type != InvoiceType.Dues)
        {
            return ResponseDto<bool>.Fail("Only dues can be added according to the apartment.");
        }

        var apartment = await unitOfWork.ApartmentRepository.AreAllApartmentsActiveAsync(request.ApartmentIds!);
        if (!apartment)
        {
            return ResponseDto<bool>.Fail("One or more apartments are either not found or not active.");
        }

        var isValidationMonthAndYear = DateHelper.IsValidMonth(request.Month) && DateHelper.IsValidYear(request.Year);
        if (!isValidationMonthAndYear)
        {
            return ResponseDto<bool>.Fail("Invalid month or year.");
        }

        foreach (var apartmentId in request.ApartmentIds!)
        {
            var dueDate = DateHelper.CalculateDueDate(request.Year, request.Month);

            var invoice = new Invoice
            {
                Type = request.Type,
                Amount = request.Amount,
                PayableAmount = request.Amount,
                Year = request.Year,
                Month = request.Month,
                PaymentStatus = false,
                ApartmentId = apartmentId,
                DueDate = dueDate
            };

            await unitOfWork.InvoiceRepository.AddInvoiceAsync(invoice);
        }

        return ResponseDto<bool>.Success(true);

    }

    public async Task<ResponseDto<bool>> UpdateInvoice(InvoiceUpdateRequestDto request)
    {
        if (!Enum.IsDefined(typeof(InvoiceType), request.Type))
        {
            return ResponseDto<bool>.Fail("Invalid invoice type.");
        }

        var invoice = await unitOfWork.InvoiceRepository.GetByIdAsync(request.InvoiceId);

        if (invoice == null)
        {
            return ResponseDto<bool>.Fail("Invoice not found.");
        }

        var dueDate = DateHelper.CalculateDueDate(request.Year, request.Month);

        invoice.Amount = request.Amount;
        invoice.PayableAmount = request.Amount;
        invoice.Year = request.Year;
        invoice.Month = request.Month;
        invoice.PaymentStatus = request.PaymentStatus;
        invoice.DueDate = dueDate;

        var updateResult = await unitOfWork.InvoiceRepository.UpdateInvoiceAsync(invoice);
        if (!updateResult)
        {
            return ResponseDto<bool>.Fail("Unable to update invoice.");
        }

        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<bool?>> DeleteInvoice(int invoiceId)
    {
        var invoice = await unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);

        if (invoice == null)
        {
            return ResponseDto<bool?>.Fail("Invoice not found.");
        }

        var deleteResult = await unitOfWork.InvoiceRepository.DeleteInvoiceAsync(invoiceId);
        if (!deleteResult)
        {
            return ResponseDto<bool?>.Fail("Unable to delete invoice.");
        }

        return ResponseDto<bool?>.Success(true);
    }
}