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
        public async void AddCartItemAsync(ShoppingCartItems item)
        {
            using (var transaction = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbcontext.ShoppingCartItems.AddAsync(item);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            
        }

        public async void DeleteCartItemAsync(string productId)
        {
            var item = await _dbcontext.ShoppingCartItems.FindAsync(productId);
            if (item != null)
            {
                _dbcontext.ShoppingCartItems.Remove(item);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<ShoppingCarts> GetByUserIdAsync(string userId)
        {
            return await _dbcontext.ShoppingCarts.Include(p=>p.ShoppingCartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(cart => cart.AppUserId == userId);
        }

        public async void UpdateCartItemAsync(ShoppingCartItems item)
        {
            _dbcontext.ShoppingCartItems.Update(item);
            await _dbcontext.SaveChangesAsync();
        }
    }

}
