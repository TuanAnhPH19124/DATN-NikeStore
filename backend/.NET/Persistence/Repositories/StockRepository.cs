using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock> SelectById(string productId)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);
        }

        public async Task AddAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
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
                }
                catch (System.Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteByProductIdAsync(string productId)
        {

            var stocksToDelete = await _context.Stocks.Where(stock => stock.ProductId == productId).ToListAsync();
            if (stocksToDelete == null || stocksToDelete.Count == 0)
            {
                throw new Exception("Không tìm thấy sản phẩm");
            }
            _context.Stocks.RemoveRange(stocksToDelete);
            await _context.SaveChangesAsync();
        }

        // Các phương thức khác tại đây...
    }
}
