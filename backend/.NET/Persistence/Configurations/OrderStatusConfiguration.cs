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
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Time).IsRequired();
            builder.Property(p => p.Note).IsRequired();

            builder.HasOne(p => p.Order)
                .WithMany(p => p.OrderStatuses)
                .HasForeignKey(p => p.OrderId);
        }
    }
}
