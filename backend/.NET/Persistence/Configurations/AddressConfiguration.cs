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
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Line).IsRequired();
            builder.Property(p => p.District).IsRequired();
            builder.Property(p => p.Province).IsRequired();
            builder.Property(p => p.Ward).IsRequired();
            builder.Property(p => p.ProvinceId).IsRequired();
            builder.Property(p => p.toDistrictId).IsRequired();
            builder.Property(p => p.WardCode).IsRequired();

            builder.HasOne(p => p.AppUser)
                .WithMany(p => p.Addresses)
                .HasForeignKey(p => p.UserId);
        }
    }
}
