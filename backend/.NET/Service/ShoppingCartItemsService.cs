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

        public async Task RemoveProductFromCartItemAsync(string cartItemId, string productId)
        {
           await _repositoryManager.ShoppingCartItemRepository.RemoveProductFromCartItemAsync(cartItemId, productId);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<Data.ShoppingCartItemData>> GetByUserIdAsync(string userId)
        {
            var cart =  await _repositoryManager.ShoppingCartItemRepository.GetByUserIdAsync(userId);
            if (cart != null)
            {
                var listCarts = cart.ShoppingCartItems.Adapt<List<Data.ShoppingCartItemData>>();
                return listCarts;
            }
            return null;
        }

        public async Task UpdatePutAsync(string Id, Boolean isQuantity)
        {
            var item = await _repositoryManager.ShoppingCartItemRepository.GetByIdCartItemAsync(Id);
            if (isQuantity)
            {
                item.Quantity++;
            }
            else
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }                           
            }
            await _repositoryManager.ShoppingCartItemRepository.UpdateCartItemAsync(Id, item);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }
        public async Task UpdateCartItemAsync(string Id, ShoppingCartItems shoppingCartItems)
        {
           await _repositoryManager.ShoppingCartItemRepository.UpdateCartItemAsync(Id, shoppingCartItems);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<ShoppingCartItems> checkProduct(string productId, string ShoppingCartId)
        {
          var check = await _repositoryManager.ShoppingCartItemRepository.CheckProductAsync(productId, ShoppingCartId);
            return check;
        }
    }
}
