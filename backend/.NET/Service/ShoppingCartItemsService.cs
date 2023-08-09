using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using EntitiesDto.Datas;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class ShoppingCartItemsService : IShoppingCartItemsService
    {
        private IRepositoryManger _repositoryManager;
        public ShoppingCartItemsService(IRepositoryManger repositoryManager)
        {
            _repositoryManager = repositoryManager;   
        }
        public async Task AddCartItemAsync(ShoppingCartItems item)
        {
            _repositoryManager.ShoppingCartItemRepository.AddCartItemAsync(item);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
            
        }

        public async Task DeleteCartItemAsync(string productId)
        {
            _repositoryManager.ShoppingCartItemRepository.DeleteCartItemAsync(productId);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<Data.ShoppingCartItemData>> GetByUserIdAsync(string userId)
        {
            var cart =  await _repositoryManager.ShoppingCartItemRepository.GetByUserIdAsync(userId);
            var listCarts = cart.ShoppingCartItems.Adapt<List<Data.ShoppingCartItemData>>();
            return listCarts;
        }

        public async Task UpdateCartItemAsync(ShoppingCartItems item)
        {
            _repositoryManager.ShoppingCartItemRepository.UpdateCartItemAsync(item);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }
    }
}
