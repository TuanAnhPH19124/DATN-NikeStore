using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
  internal sealed class OrderRepository : IOrderRepository
  {
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task Post(Order order)
    {
      using (var transaction = await _context.Database.BeginTransactionAsync())
      {
        try
        {
          await _context.AddAsync(order);
          await _context.SaveChangesAsync();
          await transaction.CommitAsync();
        }
        catch (System.Exception)
        {
          await transaction.RollbackAsync();
          throw;
        }
      }
    }

        public async Task<List<Order>> GetAllOrderAsync(CancellationToken cancellationToken = default)
        {
            var orderList = await _context.Orders.ToListAsync(cancellationToken);
            return orderList;
        }
        public async Task<Order> SelectById(string id)
    {
      return await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<string> Update(string id, Order order)
    {
      using (var transaction = await _context.Database.BeginTransactionAsync()){
        try
        {
          var currentOrder = await SelectById(id);
          currentOrder.Status = order.Status;
          currentOrder.ModifiedDate = order.ModifiedDate;
          _context.Orders.Update(currentOrder);
          await _context.SaveChangesAsync();
          await transaction.CommitAsync();
          return currentOrder.Id;
        }
        catch (System.Exception)
        {
          await transaction.RollbackAsync();
          throw;
        }
      }
    }
  }
}