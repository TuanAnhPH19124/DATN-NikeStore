using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOrderStatusRepository
    {
        Task<IEnumerable<OrderStatus>> GetAllAsync();
        Task<OrderStatus> GetByIdAsync(string id);
        Task<IEnumerable<OrderStatus>> GetByOrderIdAsync(string orderId);
        Task AddAsync(OrderStatus orderStatus);
        Task UpdateAsync(OrderStatus orderStatus);
        Task DeleteAsync(OrderStatus orderStatus);
    }
}
