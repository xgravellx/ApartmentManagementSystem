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
    public async Task<ResponseDto<bool?>> CreateGeneralInvoice(InvoiceCreateGeneralRequestDto request)
    {
        if (!Enum.IsDefined(typeof(InvoiceType), request.Type))
        {
            return ResponseDto<bool?>.Fail("Invalid invoice type.");
        }

        if (request.Type == InvoiceType.Dues)
        {
            return ResponseDto<bool?>.Fail("You can create dues for each apartment.");
        }

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

    // 	Daire başına ödenmesi gereken aidat bilgilerini toplu olarak veya tek tek  dairelere atama yaparak gerçekleştirebilir. 
    public async Task<ResponseDto<bool?>> CreateDuesInvoice(InvoiceCreateDuesRequestDto request)
    {
        if (request.Type != InvoiceType.Dues)
        {
            return ResponseDto<bool?>.Fail("Only dues can be added according to the apartment.");
        }

        var apartment = await unitOfWork.ApartmentRepository.AreApartmentIdsExistAsync(request.ApartmentIds!);
        if (!apartment)
        {
            return ResponseDto<bool?>.Fail("Apartments not found.");
        }

        foreach (var apartmentId in request.ApartmentIds!)
        {
            var invoice = new Invoice
            {
                Type = request.Type,
                Amount = request.Amount,
                Year = request.Year,
                Month = request.Month,
                PaymentStatus = false,
                ApartmentId = apartmentId
            };

            await unitOfWork.InvoiceRepository.AddInvoiceAsync(invoice);
        }
        
        return ResponseDto<bool?>.Success(true);
    }

    // block a göre genel fatura güncelleme 
    public async Task<ResponseDto<bool?>> UpdateInvoice(InvoiceUpdateRequestDto request)
    {
        if (!Enum.IsDefined(typeof(InvoiceType), request.Type))
        {
            return ResponseDto<bool?>.Fail("Invalid invoice type.");
        }

        var invoice = await unitOfWork.InvoiceRepository.GetByIdAsync(request.InvoiceId);

        // Fatura bulunamazsa hata dön.
        if (invoice == null)
        {
            return ResponseDto<bool?>.Fail("Invoice not found.");
        }

        invoice.Amount = request.Amount;
        invoice.Year = request.Year;
        invoice.Month = request.Month;
        invoice.PaymentStatus = request.PaymentStatus;

        // Fatura güncellemesini veritabanına kaydet.
        var updateResult = await unitOfWork.InvoiceRepository.UpdateInvoiceAsync(invoice);
        if (!updateResult)
        {
            return ResponseDto<bool?>.Fail("Unable to update invoice.");
        }

        return ResponseDto<bool?>.Success(true);
    }

    // fatura silme
    public async Task<ResponseDto<bool?>> DeleteInvoice(int invoiceId)
    {
        var invoice = await unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);

        // Fatura bulunamazsa hata dön.
        if (invoice == null)
        {
            return ResponseDto<bool?>.Fail("Invoice not found.");
        }

        // Fatura silme işlemini gerçekleştir.
        var deleteResult = await unitOfWork.InvoiceRepository.DeleteInvoiceAsync(invoiceId);
        if (!deleteResult)
        {
            return ResponseDto<bool?>.Fail("Unable to delete invoice.");
        }

        return ResponseDto<bool?>.Success(true);
    }



    // fatura atama işlemleri

}