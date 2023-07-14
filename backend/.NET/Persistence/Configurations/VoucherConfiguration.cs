using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.Value).IsRequired();
            builder.Property(p => p.Description).IsRequired();

            builder.HasMany(p => p.Orders)
                .WithOne(p => p.Voucher)
                .HasForeignKey(p => p.VoucherId);

        }
    }
}
