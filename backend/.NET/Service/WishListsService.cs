using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class WishListsService : IWishListsService
    {
        private readonly IRepositoryManger _repositoryManger;

        public WishListsService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        public async Task CreateAsync(WishLists wishLists, CancellationToken cancellationToken = default)
        {

            await _repositoryManger.WishListsRepository.AddItem(wishLists);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> IsWishListExists(WishLists wish)
        {
            var wishListExsist = await _repositoryManger.WishListsRepository.GetItemByAppUserIDAndProductID(wish.AppUserId, wish.ProductsId);
            if (wishListExsist == null)
            {
                return false;
            }
            return true;
        }

        public async Task<WishLists> DeleteAsync(string appUserId, string productId, CancellationToken cancellationToken = default)
        {
            var item = await _repositoryManger.WishListsRepository.GetItemByAppUserIDAndProductID(appUserId, productId, cancellationToken);
            if (item != null)
            {
                _repositoryManger.WishListsRepository.RemoveItem( appUserId,productId);
                await _repositoryManger.UnitOfWork.SaveChangeAsync();
            }
            return item;
        }

        public async Task<List<WishLists>> GetItemsByUserID(string AppUserId, CancellationToken cancellationToken = default)
        {
            var items = _repositoryManger.WishListsRepository.GetItemsByUserID(AppUserId);

            return await items;
        }
        public async Task<WishLists> GetItemByAppUserIDAndProductID(string appUserId, string productId, CancellationToken cancellationToken = default)
        {
            var item = _repositoryManger.WishListsRepository.GetItemByAppUserIDAndProductID(appUserId, productId, cancellationToken);
            return await item;
        }

    }
}
