using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
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
        public async Task<decimal> GetTotalRevenueInTimeRange(DateTime startDate, DateTime endDate)
        {
            decimal totalRevenue = (decimal)await _context.OrderItems
                .Where(p => p.OrderDate >= startDate && p.OrderDate <= endDate)
                .SumAsync(p => p.Quantity * p.UnitPrice);
            return totalRevenue;
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
    }
}