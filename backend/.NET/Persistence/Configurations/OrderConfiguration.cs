using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Address).IsRequired();
            builder.Property(p => p.PhoneNumber).IsRequired();
            builder.Property(p => p.PhoneNumber).HasMaxLength(10);
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Note).IsRequired(false);
            builder.Property(p => p.Paymethod).IsRequired();
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.CustomerName).IsRequired(false);
            builder.Property(p => p.DateCreated).IsRequired();
            builder.Property(p => p.VoucherId).IsRequired(false);
            builder.Property(p => p.UserId).IsRequired(false);


            builder.HasOne(p => p.AppUser)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.UserId);

            builder.HasOne(p => p.Voucher)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.VoucherId);

            builder.HasMany(p => p.OrderItems)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId);
        }
    }
}