using Domain.DTOs;
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

        public async Task<ShoppingCartItems> AddToCart(ShoppingCartItemAPI item)
        {
            var checkCartExist = await _repositoryManager.ShoppingCartItemRepository.GetByUserIdAndStockId(item.AppUserId, item.StockId);
            var newItem = new ShoppingCartItems();
            if (checkCartExist != null)
            {
                checkCartExist.Quantity += item.Quantity;
                _repositoryManager.ShoppingCartItemRepository.Update(checkCartExist);
                newItem = checkCartExist;
            }
            else
            {
                newItem.Quantity = item.Quantity;
                newItem.StockId = item.StockId;
                newItem.AppUserId = item.AppUserId;
                await _repositoryManager.ShoppingCartItemRepository.Add(newItem);
            }
           
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
            return newItem;
        }

        public async Task DeleteCart(string id)
        {
            var targetItem = await _repositoryManager.ShoppingCartItemRepository.GetById(id);
            if (targetItem == null)
                throw new Exception("Không tìm thấy giỏ hàng này.");

            _repositoryManager.ShoppingCartItemRepository.Delete(targetItem);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<ShoppingCartDto>> GetByUserId(string userId)
        {
            var carts = await _repositoryManager.ShoppingCartItemRepository.GetByUserId(userId);

            var cartsDto = carts.Select(p => new ShoppingCartDto
            {
                Id = p.Id,
                Quantity = p.Quantity,
                ColorName = p.Stock.Color.Name,
                SizeId = p.Stock.SizeId,
                Product = new ShoppingCartProductDto
                {
                    Id = p.Stock.ProductId,
                    Name = p.Stock.Product.Name,
                    DiscountRate = p.Stock.Product.DiscountRate,
                    RetailPrice = p.Stock.Product.RetailPrice,
                    ImgUrl = p.Stock.Product.ProductImages.FirstOrDefault(a => a.ColorId == p.Stock.ColorId).ImageUrl
                }
            }).ToList();
            return cartsDto;
        }

        public async Task UpdateQuantity(ShoppingCartItems item)
        {
            var targetItem = await _repositoryManager.ShoppingCartItemRepository.GetById(item.Id);

            if (targetItem == null)
                throw new Exception("Có gì đó không đúng, không tìm thầy giỏ hàng này");

            targetItem.Quantity = item.Quantity;

            _repositoryManager.ShoppingCartItemRepository.Update(targetItem);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }
    }
}
