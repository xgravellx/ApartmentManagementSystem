﻿using Microsoft.AspNetCore.Identity;

namespace ApartmentManagementSystem.Models.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public bool Regular { get; set; }

    // Navigation properties
    public virtual ICollection<Apartment> Apartment { get; set; }
    public virtual ICollection<Payment> Payment { get; set; }
}