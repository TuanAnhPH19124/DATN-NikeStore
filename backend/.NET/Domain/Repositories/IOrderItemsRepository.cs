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
    }
}