using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ShoppingCartsService : IShoppingCartsService
    {
        private IRepositoryManger _repositoryManager;
        public ShoppingCartsService(IRepositoryManger repositoryManager)
        {
            _repositoryManager = repositoryManager;   
        }
        public async Task AddCartItemAsync(ShoppingCartItems item)
        {
            _repositoryManager.ShoppingCartsRepository.AddCartItemAsync(item);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
            
        }

        public async Task DeleteCartItemAsync(string productId)
        {
            _repositoryManager.ShoppingCartsRepository.DeleteCartItemAsync(productId);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<ShoppingCarts> GetByUserIdAsync(string userId)
        {
            return await _repositoryManager.ShoppingCartsRepository.GetByUserIdAsync(userId);
        }

        public async Task UpdateCartItemAsync(ShoppingCartItems item)
        {
            _repositoryManager.ShoppingCartsRepository.UpdateCartItemAsync(item);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }
    }
}
