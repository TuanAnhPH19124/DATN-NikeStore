using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly AppDbContext _context;

        public OrderItemsRepository(AppDbContext context)
        {
            _context = context;
        }       

        public async Task<IEnumerable<OrderItem>> SelectItemByOrderId(string OrderId)
        {
            return await _context.OrderItems.Where(p => p.OrderId == OrderId).ToListAsync();
        }


        // doanh so ban hang trong 1 khoang thoi gian
        public async Task<float> GetTotalOrders(DateTime startDate, DateTime endDate)
        {
            var orders = await _context.Orders
                .Where(p => p.DateCreated >= startDate && p.DateCreated <= endDate)
                .ToListAsync();
            float totalRevenue = (float)orders
                .Select(p => p.Amount)
                .DefaultIfEmpty(0)
                .Sum();

            return (totalRevenue);
        }

        // doanh so ban hang cua thang hien tai
        public async Task<float> GetTotalOrdersForCurrentMonth()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var orders = await _context.Orders
                .Where(p => p.DateCreated >= startDate && p.DateCreated <= endDate)
                .ToListAsync();

            float totalAmount = (float)orders
                .Select(p => p.Amount)
                .DefaultIfEmpty(0)
                .Sum();

            return totalAmount;
        }
        // doanh so ban hang cua ngay hom nay
        public async Task<float> GetTotalOrdersForCurrentDate()
        {
            DateTime today = DateTime.Today;

            var orders = await _context.Orders
                .Where(p => p.DateCreated.Date == today)
                .ToListAsync();

            float totalAmount = (float)orders
                .Select(p => p.Amount)
                .DefaultIfEmpty(0)
                .Sum();

            return totalAmount;
        }

        // Tong so don hang cua ngay hom nay
        public async Task<int> GetTotalBillForCurrentDate()
        {
            DateTime today = DateTime.Today;

            var orders = await _context.Orders
                .Where(p => p.DateCreated.Date == today)
                .ToListAsync();

            int totalOrders = orders.Count;

            return totalOrders;
        }

        // Tong so don hang cua thang hien tai
        public async Task<int> GetTotalBillForCurrentMonth()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var orders = await _context.Orders
                .Where(p => p.DateCreated >= startDate && p.DateCreated <= endDate)
                .ToListAsync();

            int totalOrders = orders.Count;

            return totalOrders;
        }

        // Tong so hoa don trong 1 khoang thoi gian
        public async Task<int> GetTotalOrdersInTimeRange(DateTime startDate, DateTime endDate)
        {
            var orders = await _context.Orders
                .Where(p => p.DateCreated >= startDate && p.DateCreated <= endDate)
                .ToListAsync();

            int totalOrders = orders.Count;

            return totalOrders;
        }

        // Tong so hoa don da ban
        public async Task<int> GetTotalBill()
        {
            var totalOrders = await _context.Orders.CountAsync();
            return totalOrders;
        }

        // Tong so doanh thu
        public async Task<float> GetTotalAmount()
        {
            var orders = await _context.Orders
            .Select(p => (float)p.Amount)
            .ToListAsync();

            float totalAmount = orders.Any() ? orders.Sum() : 0;

            return totalAmount;
        }

        //So luong da ban cua 1 sp
        public async Task<int> GetAmountByProductId(string productId)
        {
            int amount = await _context.OrderItems
                .Where(p => p.ProductId == productId).SumAsync(p => p.Quantity);
            return amount;
        }

        //doanh thu cua 1 sp 
        public async Task<decimal> GetRevenueByProductId(string productId)
        {
            decimal revenue = (decimal)await _context.OrderItems
                .Where(p => p.ProductId == productId)
                .SumAsync(p => p.Quantity * p.UnitPrice);
            return revenue;
        }

        // select 5 san pham ban duoc nhieu nhat
        public async Task<List<ProductSalesAndRevenueInfo>> GetTopSellingProductsAndRevenue(int topCount)
        {
            // Lấy danh sách sản phẩm bán được và doanh thu
            var topProducts = await _context.OrderItems
                .GroupBy(p => p.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(p => p.Quantity),
                    TotalRevenue = g.Sum(p => p.Quantity * p.UnitPrice)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(topCount)
                .ToListAsync();

            // Lấy danh sách các ProductId cần truy vấn
            var productIds = topProducts.Select(item => item.ProductId).ToList();

            // Truy vấn tên sản phẩm từ bảng Products
            var productNames = await _context.Products.Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Name);

            // Tạo danh sách kết quả
            var results = topProducts
                .Select(item => new ProductSalesAndRevenueInfo
                {
                    ProductId = item.ProductId,
                    ProductName = productNames.ContainsKey(item.ProductId) ? productNames[item.ProductId] : null,
                    TotalQuantitySold = item.TotalQuantitySold,
                    TotalRevenue = (decimal)item.TotalRevenue
                })
                .ToList();

            return results;
        }



    }
}