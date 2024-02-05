using ApartmentManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Apartment> Apartment { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<Payment> Payment { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apartment ve User arasındaki bire-çok ilişkiyi yapılandırma.

        // Invoice ve Apartment arasındaki bire-çok ilişkiyi yapılandırma.
        
        // Payment, User ve Invoice ile olan ilişkilerini yapılandırma.

        // Eğer ek yapılandırmalar (indeksler, sınırlamalar vb.) varsa burada tanımlayın.
        // Örneğin, kullanıcı adının ve e-posta adresinin benzersiz olması gibi.
    }

}