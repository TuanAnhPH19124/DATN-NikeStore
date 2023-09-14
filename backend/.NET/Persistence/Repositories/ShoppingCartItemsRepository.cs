using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class ShoppingCartItemsRepository : IShoppingCartItemRepository
    {
        private AppDbContext _dbcontext;
        public ShoppingCartItemsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task Add(ShoppingCartItems item)
        {
            await _dbcontext.ShoppingCartItems.AddAsync(item);
        }

        public async Task AddRange(List<ShoppingCartItems> items)
        {
            await _dbcontext.ShoppingCartItems.AddRangeAsync(items);
        }

        public void Delete(ShoppingCartItems item)
        {
            _dbcontext.ShoppingCartItems.Remove(item);
        }

        public void DeleteRange(List<ShoppingCartItems> items)
        {
            _dbcontext.ShoppingCartItems.RemoveRange(items);
        }

        public async Task<IEnumerable<ShoppingCartItems>> GetAllById(string userId)
        {
            return await _dbcontext.ShoppingCartItems.Where(p => p.AppUserId == userId).ToListAsync();
        }

        public async Task<ShoppingCartItems> GetById(string id)
        {
            return await _dbcontext.ShoppingCartItems.FindAsync(id);
        }

        public async Task<IEnumerable<ShoppingCartItems>> GetByUserId(string userId)
        {
            return await _dbcontext.ShoppingCartItems.Include(p => p.Stock.Product).ThenInclude(p => p.ProductImages).Include(p => p.Stock.Color).Include(p => p.Stock.Size).Where(p => p.AppUserId == userId).ToListAsync();
        }

        public async Task<ShoppingCartItems> GetByUserIdAndStockId(string userId, string stockId)
        {
            return await _dbcontext.ShoppingCartItems.FirstOrDefaultAsync(p => p.AppUserId == userId && p.StockId == stockId);
        }

        public void Update(ShoppingCartItems item)
        {
            _dbcontext.ShoppingCartItems.Update(item);
        }
    }

}
