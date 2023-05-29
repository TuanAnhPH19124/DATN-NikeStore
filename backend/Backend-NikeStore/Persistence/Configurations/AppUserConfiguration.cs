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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(nameof(AppUser));
            builder.HasKey(user => new { user.Id, user.Email});

            builder.Property(user => user.Id).ValueGeneratedOnAdd();
            builder.Property(user => user.PhoneNumber).HasMaxLength(10);
            builder.Property(user => user.FullName).HasMaxLength(50);
        }
    }
}
