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
    // fatura listesi
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetAll()
    {
        var invoices = await unitOfWork.InvoiceRepository.GetAllAsync();

        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices ?? []);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);
    }

    // apartment id ye göre faturaları getir
    // todo -> optimize etmelisin
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(int apartmentId, string userId, bool isAdmin)
    {
        if (!isAdmin)
        {
            var userIdFindUser = await userManager.Users.FirstOrDefaultAsync(u => u.IdentityNumber == userId);
            var userIdByUserId = userIdFindUser!.Id;
            var apartmentIdByUser = await unitOfWork.ApartmentRepository.GetApartmentIdsByUserIdAsync(userIdByUserId);
            var invoicesByUser = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(apartmentIdByUser);
            var invoiceDtoListByUser = mapper.Map<List<InvoiceResponseDto>>(invoicesByUser);
            return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoListByUser);

        }

        var invoices = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(apartmentId);
        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);

    }

    // filtrelenmiş fatura listesi 
    // todo: ödenmişse fatura bilgilerini de gönder
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

        var activeApartments = await unitOfWork.ApartmentRepository.GetActiveApartmentsByBlockAsync(request.Block);
        if (!activeApartments.Any())
        {
            return ResponseDto<bool?>.Fail("No active apartments found for the block.");
        }

        var totalApartments = activeApartments.Count;
        var amountPerApartment = request.Amount / totalApartments;

        foreach (var apartment in activeApartments)
        {
            var dueDate = InvoiceHelper.CalculateDueDate(request.Year, request.Month);

            var invoice = new Invoice
            {
                Type = request.Type,
                Amount = amountPerApartment,
                Year = request.Year,
                Month = request.Month,
                PaymentStatus = false,
                ApartmentId = apartment.ApartmentId,
                DueDate = dueDate
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

        var dueDate = InvoiceHelper.CalculateDueDate(request.Year, request.Month);

        invoice.Amount = request.Amount;
        invoice.Year = request.Year;
        invoice.Month = request.Month;
        invoice.PaymentStatus = request.PaymentStatus;
        invoice.DueDate = dueDate;

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


    // todo: test etmelisin.
    public async Task CheckAndApplyOverDue()
    {
        var today = DateTime.UtcNow.AddHours(3);
        var invoicesToUpdate = await unitOfWork.InvoiceRepository.GetUnpaidAndOverdueInvoicesAsync(today);

        foreach (var invoice in invoicesToUpdate)
        {
            // %10 ceza uygulayın
            invoice.Amount += invoice.Amount * 0.10m;
            await unitOfWork.InvoiceRepository.UpdateInvoiceAsync(invoice);
        }

        // Değişiklikleri veritabanına kaydedin
        await unitOfWork.SaveChangesAsync();
    }



    // fatura atama işlemleri

}