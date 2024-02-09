using ApartmentManagementSystem.Models.Entities;

namespace ApartmentManagementSystem.Models.Shared;

public class PaymentDto : Payment
{
    public bool IsRegular { get; set; }
}