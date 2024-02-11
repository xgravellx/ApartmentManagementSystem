using ApartmentManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApartmentManagementSystem.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.PaymentId);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Amount)
            .IsRequired();

        builder.Property(p => p.Date)
            .IsRequired();

        builder.Property(p => p.Method)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Payment)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Invoice)
            .WithMany()
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}