using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Repositories
{
     internal sealed class WishListsRepository : IWishListsRepository
     {
        private readonly AppDbContext _dbcontext;
        public WishListsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddItem(WishLists item, CancellationToken cancellationToken = default)
        {
            using (var transaction = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbcontext.WishLists.Add(item);
                    await transaction.CommitAsync();
                }
                catch (System.Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<WishLists> GetItemByAppUserIDAndProductID(string appUserId, string productId, CancellationToken cancellationToken = default)
        {
            return await _dbcontext.WishLists.FirstOrDefaultAsync(c => c.ProductsId == productId && c.AppUserId == appUserId, cancellationToken);
        }

        public async Task<List<WishLists>> GetItemsByUserID(string appUserID, CancellationToken cancellationToken = default)
        {
            var items =  _dbcontext.WishLists.Include(p => p.Product).ThenInclude(p => p.ProductImages).Where(item => item.AppUserId == appUserID).ToList();
            return await Task.FromResult(items);
        }

        public void RemoveItem(string appUserId, string productId, CancellationToken cancellationToken = default)
        {
            var item = _dbcontext.WishLists.FirstOrDefault(item => item.AppUserId == appUserId && item.ProductsId == productId);
            _dbcontext.WishLists.Remove(item);
        }
    }
}
