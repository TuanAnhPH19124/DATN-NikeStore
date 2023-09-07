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
            builder.Property(p => p.FullName).IsRequired();
            builder.Property(p => p.AddressLine).IsRequired();
            builder.Property(p => p.WardCode).IsRequired();
            builder.Property(p => p.CityCode).IsRequired();
            builder.Property(p => p.ProvinceCode).IsRequired();
            builder.Property(p => p.PhoneNumber).IsRequired();
            builder.Property(p => p.SetAsDefault).IsRequired();

            builder.HasOne(p => p.AppUser)
                .WithMany(p => p.Addresses)
                .HasForeignKey(p => p.UserId);

            builder.HasMany(o => o.Orders)
                .WithOne(a => a.address)
                .HasForeignKey(o => o.AddressId);
        }
    }
}
