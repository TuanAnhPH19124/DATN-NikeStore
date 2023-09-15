using Domain.DTOs;
using Domain.Entities;
using EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IWishListsService
    {
        Task<WishLists> GetItemByAppUserIDAndProductID(string appUserId, string productId, CancellationToken cancellationToken = default);
        Task CreateAsync(WishLists wishLists, CancellationToken cancellationToken = default);
        Task<List<WishListDto>> GetItemsByUserID(string AppUserId, CancellationToken cancellationToken = default);

        Task<WishLists> DeleteAsync(string appUserId, string productId, CancellationToken cancellationToken = default);
        Task<bool> IsWishListExists(WishLists wish);
    }
}
