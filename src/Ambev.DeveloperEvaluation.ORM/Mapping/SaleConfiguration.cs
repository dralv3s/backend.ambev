using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.SaleNumber).IsRequired().HasMaxLength(50);
        builder.Property(u => u.SaleDate).IsRequired();
        builder.Property(u => u.Customer).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Branch).HasMaxLength(20);
        builder.Property(u => u.IsCancelled);

        builder.Property(u => u.TotalSaleAmount).HasColumnType("decimal(18,2)");

        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey(si => si.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
