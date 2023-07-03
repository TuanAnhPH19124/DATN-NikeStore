using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable(nameof(Stock));
            builder.HasKey(p => p.StockId);
            builder.Property(p => p.UnitInStock).IsRequired();

            builder.HasOne(s => s.Product)
               .WithMany(p => p.Stocks)
               .HasForeignKey(s => s.ProductId);

            builder.HasOne(s => s.Color)
                   .WithMany(c => c.Stocks)
                   .HasForeignKey(s => s.ColorId);

            builder.HasOne(s => s.Size)
                   .WithMany(s => s.Stocks)
                   .HasForeignKey(s => s.SizeId);
        }
    }
}
