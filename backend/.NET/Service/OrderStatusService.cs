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
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IRepositoryManger _repositoryManger;
        public OrderStatusService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }
        public async Task AddAsync(OrderStatus orderStatus)
        {
            if (orderStatus.Status == Domain.Enums.StatusOrder.CANCELED)
            {
                var order = await _repositoryManger.OrderRepository.GetByIdOrderAsync(orderStatus.OrderId);

                var StockList = new List<Stock>();
                foreach (var orderQuantity in order.OrderItems)
                {
                    var currentUnitOfStock = await _repositoryManger.StockRepository.SelectByVariantId(orderQuantity.ProductId, orderQuantity.ColorId, orderQuantity.SizeId);
                    if (currentUnitOfStock == null) throw new System.Exception($"There are something wrong! Could not find the stock with ProducId {orderQuantity.ProductId}, ColorId {orderQuantity.ColorId}, SizeId {orderQuantity.SizeId}.");
                    currentUnitOfStock.UnitInStock += orderQuantity.Quantity;
                    StockList.Add(currentUnitOfStock);
                }

                _repositoryManger.StockRepository.UpdateRange(StockList);

            }

            await _repositoryManger.OrderStatusRepository.AddAsync(orderStatus);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
        }
    }
}
