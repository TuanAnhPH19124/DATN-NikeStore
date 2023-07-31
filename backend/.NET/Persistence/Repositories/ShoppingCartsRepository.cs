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
    internal sealed class ShoppingCartsRepository : IShoppingCartsRepository
    {
        private AppDbContext _dbcontext;
        public ShoppingCartsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async void AddCartItemAsync(ShoppingCartItems item)
        {
            _dbcontext.ShoppingCartItems.Add(item);
            await _dbcontext.SaveChangesAsync();
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
            return await _dbcontext.ShoppingCarts
             .Include(cart => cart.Items)
             .ThenInclude(item => item.Product)
             .FirstOrDefaultAsync(cart => cart.AppUserId == userId);
        }

        public async void UpdateCartItemAsync(ShoppingCartItems item)
        {
            _dbcontext.ShoppingCartItems.Update(item);
            await _dbcontext.SaveChangesAsync();
        }
    }

}
