using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _dbContext;

        public ShoppingCartRepository(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task<string> GetShoppingCartIdByUserIdAsync(string userId)
        {
            var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(cart => cart.AppUserId == userId);
            if (shoppingCart == null)
            {
                return null;
            }
            return shoppingCart.Id;
        }

        public async Task<bool> CheckUserCart(string userId)
        {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(p=>p.AppUserId == userId);
            if (cart == null)
                return false;
            return true;
        }

        public async Task AddAsync(ShoppingCarts cart)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.ShoppingCarts.AddAsync(cart);
                    await transaction.CommitAsync();

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public Task DeleteAsync(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
