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
            return await _dbcontext.ShoppingCartItems.Include(p => p.Product).ThenInclude(p => p.ProductImages).Include(p => p.Color).Include(p => p.Size).Where(p => p.AppUserId == userId).ToListAsync();
        }

        public async Task<ShoppingCartItems> GetById(string id)
        {
            return await _dbcontext.ShoppingCartItems.FindAsync(id);
        }

        public async Task<ShoppingCartItems> GetByRelationId(string userId, string productId, string colorId, string sizeId)
        {
            return await _dbcontext.ShoppingCartItems.FirstOrDefaultAsync(p => p.AppUserId == userId && p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId);
        }

        public async Task<IEnumerable<ShoppingCartItems>> GetByUserId(string userId)
        {
            return await _dbcontext.ShoppingCartItems.Include(p => p.Product).ThenInclude(p => p.ProductImages).Include(p => p.Color).Include(p => p.Size).Where(p => p.AppUserId == userId).ToListAsync();
        }

        public void Update(ShoppingCartItems item)
        {
            _dbcontext.ShoppingCartItems.Update(item);
        }

        public void UpdateRange(List<ShoppingCartItems> items)
        {
            _dbcontext.ShoppingCartItems.UpdateRange(items);
        }
    }

}
