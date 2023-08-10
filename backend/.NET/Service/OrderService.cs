using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Order;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class OrderService : IOrderService
    {
        private readonly IRepositoryManger _manager;

        public OrderService(IRepositoryManger manager)
        {
            _manager = manager;
        }

        public async Task PostAndSendNontification(OrderPostRequestDto orderDto)
        {
            var order = orderDto.Adapt<Order>();
            order.OrderItems = orderDto.OrderItems.Adapt<List<OrderItem>>(); 
            await _manager.OrderRepository.Post(order);
        }

        public async Task UpdateOrderOnConfirm(string id, object order)
        {
            try
            {
                #region Update in table Order (Status and ModifiedDate)
                var newOrder = order.Adapt<Order>();
                var orderId = await _manager.OrderRepository.Update(id, newOrder);
                #endregion
                #region Update UnitOfStock in Stock table
                var orderQuanities = await _manager.OrderItemsRepository.SelectItemByOrderId(orderId);
                var StockList = new List<Stock>();
                foreach (var orderQuantity in orderQuanities)
                {
                    var currentUnitOfStock = await _manager.StockRepository.SelectById(orderQuantity.ProductId);
                    if (currentUnitOfStock == null) throw new System.Exception($"There are something wrong! Could not find the stock with ProducId {orderQuantity.ProductId}, ColorId {orderQuantity.ColorId}, SizeId {orderQuantity.SizeId}.");
                    currentUnitOfStock.UnitInStock -= orderQuantity.Quantity;
                    StockList.Add(currentUnitOfStock);
                }
                await _manager.StockRepository.UpdateRange(StockList);
                Console.WriteLine("Update unit of stock successfully.");
                #endregion
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}