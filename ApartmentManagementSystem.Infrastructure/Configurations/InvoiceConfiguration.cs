using ApartmentManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApartmentManagementSystem.Infrastructure.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.InvoiceId);

        builder.Property(i => i.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(i => i.Amount)
            .IsRequired();

        builder.Property(i => i.Year)
            .IsRequired();

        builder.Property(i => i.Month)
            .IsRequired();

        builder.Property(i => i.PaymentStatus)
            .IsRequired();

        builder.HasOne(i => i.Apartment)
            .WithMany(a => a.Invoice)
            .HasForeignKey(i => i.ApartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}