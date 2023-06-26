using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
      
      builder.HasMany(p => p.OrderItems)
      .WithOne(p => p.Order)
      .HasForeignKey(p => p.OrderId);
    }
  }
}