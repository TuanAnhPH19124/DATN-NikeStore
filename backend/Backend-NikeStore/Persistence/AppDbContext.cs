using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> optionsBuilder) : base(optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<Schedule>()
          .HasOne(s => s.Employees)
          .WithMany()
          .HasForeignKey(s => s.EmployeeId);
            modelBuilder.Entity<WishLists>()
            .HasKey(w => new { w.ProductsId, w.AppUserId });

            modelBuilder.Entity<WishLists>()
                .HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductsId);

            modelBuilder.Entity<WishLists>()
                .HasOne(w => w.AppUser)
                .WithMany()
                .HasForeignKey(w => w.AppUserId);
            modelBuilder.Entity<ShoppingCarts>()
              .HasOne(s => s.AppUser)
              .WithOne(u => u.ShoppingCarts)
              .HasForeignKey<ShoppingCarts>(s => s.AppUserId);

            modelBuilder.Entity<ShoppingCartItems>()
              .HasKey(item => new { item.ShoppingCartsId, item.ProductsId });

            modelBuilder.Entity<ShoppingCartItems>()
             .HasOne(item => item.ShoppingCarts)
             .WithMany(cart => cart.Items) // Sử dụng tên thuộc tính đúng của ShoppingCart
             .HasForeignKey(item => item.ShoppingCartsId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCartItems>()
                .HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductsId);
            modelBuilder.Entity<Product>()
                 .HasMany(p => p.ProductRate)
                 .WithOne(pr => pr.Product)
                 .HasForeignKey(pr => pr.ProductId);

            // Cấu hình bảng "AppUser"
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.ProductRate)
                .WithOne(pr => pr.AppUser)
                .HasForeignKey(pr => pr.AppUserId);
            modelBuilder.Entity<ProductRate>()
            .HasKey(pr => new { pr.AppUserId, pr.ProductId });
        }

        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<ShoppingCartItems> ShoppingCartItems { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<ShoppingCarts> ShoppingCarts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<WishLists> WishLists { get; set; }
        public DbSet<ProductRate> ProductRate { get; set; }

    }
}
