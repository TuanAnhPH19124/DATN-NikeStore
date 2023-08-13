using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task Post(Order order);
        Task<string> Update(string id, Order order);
        Task<Order> SelectById(string id);
        Task<List<OrderDto>> GetAllOrderAsync(CancellationToken cancellationToken = default);
        Task<Order> GetByIdOrderAsync(string id, CancellationToken cancellationToken = default);
    }
}