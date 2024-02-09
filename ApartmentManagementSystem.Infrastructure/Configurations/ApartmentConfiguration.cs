using ApartmentManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApartmentManagementSystem.Infrastructure.Configurations;

public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.HasKey(a => a.ApartmentId);

        builder.Property(a => a.Block)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(a => a.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Floor)
            .IsRequired();

        builder.Property(a => a.Number)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(a => a.User)
            .WithOne(u => u.Apartment)
            .HasForeignKey<User>(u => u.ApartmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }

}