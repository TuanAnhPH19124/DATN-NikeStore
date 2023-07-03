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
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.ToTable(nameof(Size));
            builder.Property(p => p.NumberSize).IsRequired();
            builder.Property(p => p.Description).IsRequired(false);
           

            builder.HasKey(p => p.Id);
            builder.HasMany(s => s.Stocks)
                   .WithOne(s => s.Size)
                   .HasForeignKey(s => s.SizeId);
        }
    }
}
