using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ApartmentManagementSystem.Core.Services;

public class InvoiceService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : IInvoiceService
{
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

        // Veri bulunamadığında boş bir liste döndür
        var invoiceDtoList = mapper.Map<List<InvoiceResponseDto>>(invoices);
        return ResponseDto<List<InvoiceResponseDto>>.Success(invoiceDtoList ?? new List<InvoiceResponseDto>());

    }
}