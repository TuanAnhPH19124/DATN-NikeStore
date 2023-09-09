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
        Task<float> GetTotalOrders(DateTime startDate, DateTime endDate);
        Task<float> GetTotalOrdersForCurrentMonth();
        Task<float> GetTotalOrdersForCurrentDate();
        Task<int> GetTotalBillForCurrentDate();
        Task<int> GetTotalBillForCurrentMonth();
        Task<int> GetTotalOrdersInTimeRange(DateTime startDate, DateTime endDate);
        Task<int> GetTotalBill();
        Task<float> GetTotalAmount();
    }
}