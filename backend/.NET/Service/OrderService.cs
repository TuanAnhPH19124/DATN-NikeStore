using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;

namespace Service
{
  internal sealed class OrderService : IOrderService
  {
    private readonly IRepositoryManger _manager;

    public OrderService(IRepositoryManger manager)
    {
      _manager = manager;
    }

    public async Task PostAndSendNontification(Order order)
    {
      await _manager.OrderRepository.Post(order);
    }
  }
}