using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InvoiceCreateGeneralRequestDto = ApartmentManagementSystem.Models.Shared.InvoiceCreateGeneralRequestDto;

namespace ApartmentManagementSystem.Core.Services;

public class InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : IInvoiceService
{
    // fatura listesi
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetAll()
    {
        var invoices = await unitOfWork.InvoiceRepository.GetAllAsync();

        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices ?? new List<Invoice>());
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);
    }

    // apartment id ye göre faturaları getir
    // todo -> optimize etmelisin
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(int ApartmentId, string userId, bool isAdmin)
    {
        if (!isAdmin)
        {
            var userIdFindUser = await userManager.Users.FirstOrDefaultAsync(u => u.IdentityNumber == userId);
            var userIdByUserId = userIdFindUser!.Id;
            var apartmentId = await unitOfWork.ApartmentRepository.GetApartmentIdsByUserIdAsync(userIdByUserId);
            var invoicesByUser = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(apartmentId);
            var invoiceDtoListByUser = mapper.Map<List<InvoiceResponseDto>>(invoicesByUser);
            return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoListByUser);

        }

        var invoices = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(ApartmentId);
        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);

    }

    // filtrelenmiş fatura listesi
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetFiltered(InvoiceFilterRequestDto request, string userId, bool isAdmin)
    {
        if (!isAdmin)
        {
            var userIdFindUser = await userManager.Users.FirstOrDefaultAsync(u => u.IdentityNumber == userId);
            var userIdByUserId = userIdFindUser!.Id;
            var apartmentId = await unitOfWork.ApartmentRepository.GetApartmentIdsByUserIdAsync(userIdByUserId);
            request.ApartmentIds = [apartmentId];
            request.UserIds = [userIdByUserId];
        }

        var invoices = await unitOfWork.InvoiceRepository.GetFilteredAsync(request);
        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);
    }

    // block a göre genel fatura oluşturma
    public async Task<ResponseDto<bool?>> CreateGeneralInvoice(InvoiceCreateGeneralRequestDto request) // todo: eksik
    {
        var activeApartments = await unitOfWork.ApartmentRepository.GetActiveApartmentsByBlock(request.Block);
        if (!activeApartments.Any())
        {
            return ResponseDto<bool?>.Fail("No active apartments found for the block.");
        }

        var totalApartments = activeApartments.Count;
        var amountPerApartment = request.Amount / totalApartments;

        foreach (var apartment in activeApartments)
        {
            var invoice = new Invoice
            {
                Type = request.Type,
                Amount = amountPerApartment,
                Year = request.Year,
                Month = request.Month,
                PaymentStatus = false,
                ApartmentId = apartment.ApartmentId
            };

            await unitOfWork.InvoiceRepository.AddInvoiceAsync(invoice);
        }

        return ResponseDto<bool?>.Success(true);
    }

    // fatura ekleme - aylık fatura oluşturma
    // fatura güncelleme
    // fatura silme
    // fatura var mı kontrolü
    // fatura ödeme durumu güncelleme
    // fatura atama işlemleri
    // ⦁Daire başına ödenmesi gereken aidat bilgilerini  toplu olarak veya tek tek  dairelere atama yaparak gerçekleştirebilir

}