using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal sealed class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<Stock> SelectById(string productId, string colorId, string sizeId)
    {
      return await _context.Stocks.FirstOrDefaultAsync(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId);
    }

    public async Task UpdateRange(List<Stock> Stocks)
    {
      using (var transaction = await _context.Database.BeginTransactionAsync())
      {
        try
        {
            _context.Stocks.UpdateRange(Stocks);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            Console.WriteLine($"Update unit of stock success.");
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