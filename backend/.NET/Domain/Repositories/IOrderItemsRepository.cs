using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderItemsRepository
    {
        Task<IEnumerable<OrderItem>> SelectItemByOrderId(string OrderId);
        Task<int> GetAmountByProductId(string productId);
        Task<decimal> GetRevenueByProductId(string productId);
        Task<decimal> GetTotalRevenueInTimeRange(DateTime startDate, DateTime endDate);
    }
}