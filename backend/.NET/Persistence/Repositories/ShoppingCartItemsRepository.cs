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

        public async Task RemoveProductFromCartItemAsync(string cartItemId, string productId)
        {
            var cartItem = await _dbcontext.ShoppingCartItems.FirstOrDefaultAsync(p => p.ShoppingCartId == cartItemId && p.ProductId == productId);

            if (cartItem != null)
            {
                _dbcontext.ShoppingCartItems.Remove(cartItem);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<ShoppingCarts> GetByUserIdAsync(string userId)
        {
            return await _dbcontext.ShoppingCarts.Include(p=>p.ShoppingCartItems).FirstOrDefaultAsync(cart => cart.AppUserId == userId);
        }

        public async Task<ShoppingCartItems> GetByIdCartItemAsync(string Id)
        {
            return await _dbcontext.ShoppingCartItems.FindAsync(Id);
        }
        public async Task UpdateCartItemAsync(string Id, ShoppingCartItems shoppingCartItems)
        {
            var item = await _dbcontext.ShoppingCartItems.FindAsync(Id);
            item = shoppingCartItems;
            _dbcontext.ShoppingCartItems.Update(item);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<ShoppingCartItems> CheckProductAsync(string productId, string ShoppingCartId)
        {
            var check = _dbcontext.ShoppingCartItems.FirstOrDefault(p => p.ProductId == productId && p.ShoppingCartId == ShoppingCartId);
            return check;
        }
    }

}
