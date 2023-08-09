using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Nest;
using Persistence;
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

        public async Task<bool> GetById(string Id)
        {
            var isHadCart1 = await _repositoryManger.ShoppingCartRepository.CheckUserCart(Id);
            return isHadCart1;
        }

        public async Task<string> GetShoppingCartIdByUserId(string Id)
        {
            var get = await _repositoryManger.ShoppingCartRepository.GetShoppingCartIdByUserIdAsync(Id);
            return get;
        }
        public async Task AddAsync(ShoppingCarts cart)
        {
            
            var isHadCart = await _repositoryManger.ShoppingCartRepository.CheckUserCart(cart.AppUserId);
            if (isHadCart)
            {
                return;
            }
            await _repositoryManger.ShoppingCartRepository.AddAsync(cart);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
        }

        public Task DeleteAsync(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
