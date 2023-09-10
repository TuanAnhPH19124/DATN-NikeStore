using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;

namespace Service.Abstractions
{
    public interface IOrderItemService
    {
        Task<int> GetAmountByProductId(string productId);
        Task<decimal> GetRevenueByProductId(string productId);
        Task<float> GetTotalOrder(DateTime startDate, DateTime endDate);
        Task<float> GetTotalOrdersForCurrentMonth();
        Task<float> GetTotalOrdersForCurrentDate();
        Task<float> GetTotalBillForCurrentDate();
        Task<float> GetTotalBillForCurrentMonth();
        Task<float> GetTotalOrdersInTimeRange(DateTime startDate, DateTime endDate);
        Task<float> GetTotalBill();
        Task<float> GetTotalAmount();
        Task<List<ProductSalesAndRevenueInfo>> GetTopSellingProductsAndRevenue(int TopCount);
    }
}
