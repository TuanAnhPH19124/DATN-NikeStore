using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IWishListsRepository
    {
        Task<WishLists> GetItemByAppUserIDAndProductID(string appUserId, string productId, CancellationToken cancellationToken = default);
        Task<List<WishLists>> GetItemsByUserID(string appUserID, CancellationToken cancellationToken = default);
        void AddItem(WishLists item, CancellationToken cancellationToken = default);
        void RemoveItem(string appUserId, string productId, CancellationToken cancellationToken = default);
    }
}
