using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly AppDbContext _dbcontext;
        public OrderStatusRepository(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task AddAsync(OrderStatus orderStatus)
        {
            await _dbcontext.OrderStatuses.AddAsync(orderStatus);
            
        }

        public Task DeleteAsync(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderStatus>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderStatus> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderStatus>> GetByOrderIdAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }
    }
}
