using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
  public class OrderItemsRepository : IOrderItemsRepository
  {
    private readonly AppDbContext _context;

    public OrderItemsRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<OrderItem>> SelectItemByOrderId(string OrderId)
    {
      return await _context.OrderItems.Where(p => p.OrderId == OrderId).ToListAsync();
    }
  }
}