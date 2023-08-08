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
    internal sealed class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepositoryManger _repositoryManger;

        public ShoppingCartService(IRepositoryManger repositoryManger)
        {
            _repositoryManger=repositoryManger;
        }

        public async Task AddAsync(ShoppingCarts cart)
        {
            var isHadCart = await _repositoryManger.ShoppingCartRepository.CheckUserCart(cart.AppUserId);
            if (isHadCart)
            {
                return;
            }
            await _repositoryManger.ShoppingCartRepository.AddAsync(cart);
        }

        public Task DeleteAsync(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
