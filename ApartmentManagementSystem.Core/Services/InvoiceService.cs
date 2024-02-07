using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

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
    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(InvoiceByApartmentIdRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return ResponseDto<List<InvoiceResponseDto>>.Fail("User not found.");
        }

        var roles = await userManager.GetRolesAsync(user);
        bool isUserAdmin = roles.Contains("Admin");

        IEnumerable<Invoice> invoices;

        if (isUserAdmin)
        {
            invoices = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(request.ApartmentId);
        }
        else
        {
            var apartment = await unitOfWork.ApartmentRepository.GetByIdAsync(request.ApartmentId);
            if (apartment == null || apartment.UserId.ToString() != request.UserId.ToString())
            {
                return ResponseDto<List<InvoiceResponseDto>>.Fail("Access denied or apartment does not exist.");
            }

            invoices = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(request.ApartmentId);
        }

        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList ?? []);

    }

    public async Task<ResponseDto<List<InvoiceResponseDto>>> GetFiltered(InvoiceFilterRequestDto request, string userId, bool isAdmin)
    {
        // isAdmin kontrolü
        if (!isAdmin)
        {
            var apartmentIds = await unitOfWork.ApartmentRepository.GetApartmentIdsByUserId(Guid.Parse(userId));
            request.ApartmentIds = apartmentIds.ToList();

            request.UserIds = new List<Guid> { Guid.Parse(userId) };
        }

        var invoices = await unitOfWork.InvoiceRepository.GetFilteredAsync(request);

        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList);

    }
}