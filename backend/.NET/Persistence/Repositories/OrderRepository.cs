using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;

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
  }
}