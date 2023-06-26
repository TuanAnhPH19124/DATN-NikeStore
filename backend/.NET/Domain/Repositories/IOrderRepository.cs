using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task Post(Order order);
    }
}