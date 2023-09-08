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
    public class SoleConfiguration : IEntityTypeConfiguration<Sole>
    {
        public void Configure(EntityTypeBuilder<Sole> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.ModifiedDate).ValueGeneratedOnAddOrUpdate();
            builder.HasIndex(p => p.Name).IsUnique();


            builder.HasMany(p => p.Products)
                .WithOne(p => p.Sole)
                .HasForeignKey(p => p.SoleId);

        }
    }
}
