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
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(nameof(Tag));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ModifiedDate).IsRequired(false);
            builder.Property(p => p.Name).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasMany(p => p.Products)
                   .WithMany(p => p.Tags)
                   .UsingEntity(p => p.ToTable("ProductTag"));
        }
    }
}
