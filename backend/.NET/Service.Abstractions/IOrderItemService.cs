using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Service.Abstractions
{
    public interface IOrderItemService
    {
        Task<int> GetAmountByProductId(string productId);
        Task<decimal> GetRevenueByProductId(string productId);
        Task<decimal> GetTotalRevenueInTimeRange(DateTime startDate, DateTime endDate);
    }
}
