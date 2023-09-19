using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using EntitiesDto.Datas;
using Mapster;
using Nest;
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

        public async Task AddRangeToCart(List<ShoppingCartItemAPI> items)
        {
            var newListCarts = new List<ShoppingCartItems>();
            var updateListCarts = new List<ShoppingCartItems>();

            foreach (var item in items)
            {
                var checkCartExist = await _repositoryManager.ShoppingCartItemRepository.GetByRelationId(item.AppUserId, item.ProductId, item.ColorId, item.SizeId);
                if (checkCartExist != null){
                    checkCartExist.Quantity = item.Quantity;
                    updateListCarts.Add(checkCartExist);
                }
                else
                {
                    newListCarts.Add(item.Adapt<ShoppingCartItems>());
                }
            }

            if (updateListCarts.Count > 0)
                _repositoryManager.ShoppingCartItemRepository.UpdateRange(updateListCarts);

            if (newListCarts.Count > 0)
                await _repositoryManager.ShoppingCartItemRepository.AddRange(newListCarts);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }

        public async Task<ShoppingCartItems> AddToCart(ShoppingCartItemAPI item)
        {
            var checkCartExist = await _repositoryManager.ShoppingCartItemRepository.GetByRelationId(item.AppUserId, item.ProductId, item.ColorId, item.SizeId);
            var newItem = new ShoppingCartItems();
            if (checkCartExist != null)
            {
                checkCartExist.Quantity = item.Quantity;
                _repositoryManager.ShoppingCartItemRepository.Update(checkCartExist);
                newItem = checkCartExist;
            }
            else
            {
                newItem.Quantity = item.Quantity;
                newItem.ProductId = item.ProductId;
                newItem.ColorId = item.ColorId;
                newItem.SizeId = item.SizeId;
                newItem.AppUserId = item.AppUserId;
                await _repositoryManager.ShoppingCartItemRepository.Add(newItem);
            }
           
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
            return newItem;
        }

        public async Task ClearCart(string id)
        {
            var targetItems = await _repositoryManager.ShoppingCartItemRepository.GetAllById(id);
            _repositoryManager.ShoppingCartItemRepository.DeleteRange(targetItems.ToList());
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
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
                ColorName = p.Color.Name,
                SizeNumber = p.Size.NumberSize,
                SizeId = p.Size.Id,
                ColorId = p.Color.Id,
                Product = new ShoppingCartProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    DiscountRate = p.Product.DiscountRate,
                    RetailPrice = p.Product.RetailPrice,
                    ImgUrl = p.Product.ProductImages.FirstOrDefault(a => a.ColorId == p.Color.Id).ImageUrl
                }
            }).ToList();
            return cartsDto;
        }

        public async Task UpdateQuantity(ShoppingCartItemPutAPI item)
        {
            var targetItem = await _repositoryManager.ShoppingCartItemRepository.GetById(item.Id);

            if (targetItem == null)
                throw new Exception("Có gì đó không đúng, không tìm thầy giỏ hàng này");

            var stock = await _repositoryManager.StockRepository.SelectByVariantId(item.ProductId, item.ColorId, item.SizeId);
            if (item.Quantity > stock.UnitInStock)
                throw new Exception("Đã đạt giới hạn số lượng trong kho");
            targetItem.Quantity = item.Quantity;
            targetItem.SizeId = item.SizeId;

            _repositoryManager.ShoppingCartItemRepository.Update(targetItem);
            await _repositoryManager.UnitOfWork.SaveChangeAsync();
        }
    }
}
