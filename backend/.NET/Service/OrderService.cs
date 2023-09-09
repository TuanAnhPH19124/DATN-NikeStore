using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using EntitiesDto;
using EntitiesDto.Order;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<List<OrderDto>> GetAllOrderAsync(CancellationToken cancellationToken = default)
        {
            var orderList = await _manager.OrderRepository.GetAllOrderAsync(cancellationToken);
            return orderList;
        }
        public async Task CreateNewOnlineOrder(OrderPostRequestDto orderDto)
        {
            #region add new order
            var order = new Order
            {
                Address = orderDto.Address,
                PhoneNumber = orderDto.PhoneNumber,
                CustomerName = orderDto.CustomerName,
                Note = orderDto.Note,
                UserId = orderDto.UserId,
                EmployeeId = orderDto.EmployeeId,
                VoucherId = orderDto.VoucherId,
                Paymethod = orderDto.PaymentMethod,
                Amount = orderDto.Amount,
                AddressId = orderDto.AddressId,
                OrderStatuses = new List<OrderStatus>(){
                    new OrderStatus{
                        Status = StatusOrder.CONFIRM,
                        Time = DateTime.Now,
                        Note = "Chờ xác nhận"
                    }
                },
                OrderItems = orderDto.OrderItems.Select(p => new OrderItem{
                    ProductId = p.ProductId,
                    ColorId = p.ColorId,
                    SizeId = p.SizeId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            };
            await _manager.OrderRepository.Post(order);
            #endregion
            #region Update UnitOfStock in stock table
            var StockList = await GetOrderItemsQuantityAsync(order.OrderItems);
            _manager.StockRepository.UpdateRange(StockList);
            Console.WriteLine("Cập nhật số lượng sản phâm thành công.");
            #endregion
            await _manager.UnitOfWork.SaveChangeAsync();
        }

        public async Task PostNewOrderAtStore(OrderAtStorePostRequestDto orderDto)
        {
            #region Add new order
            var order = new Order
            {
                Address = orderDto.Address,
                PhoneNumber = orderDto.PhoneNumber,
                CustomerName = orderDto.CustomerName,
                Note = orderDto.Note,
                UserId = orderDto.UserId,
                EmployeeId = orderDto.EmployeeId,
                VoucherId = orderDto.VoucherId,
                Paymethod = orderDto.PaymentMethod,
                Amount = orderDto.Amount,
                
            };
            
            if (orderDto.Shipping)
            {
                order.OrderStatuses = new List<OrderStatus>()
                {
                    new OrderStatus
                    {
                        Status = Domain.Enums.StatusOrder.CONFIRM,
                        Time = DateTime.Now,
                        Note = "Chờ xác nhận"
                    }
                    //new OrderStatus
                    //{
                    //    Status = Domain.Enums.StatusOrder.PENDING_SHIP,
                    //    Time = DateTime.Now,
                    //    Note = "Chờ vận chuyển"
                    //},
                    //new OrderStatus
                    //{
                    //    Status = Domain.Enums.StatusOrder.SHIPPING,
                    //    Time = DateTime.Now,
                    //    Note = "Đang vận chuyển"
                    //},
                    //new OrderStatus
                    //{
                    //    Status = Domain.Enums.StatusOrder.DELIVERIED,
                    //    Time = DateTime.Now,
                    //    Note = "Thành công"
                    //},
                };
            }
           else
            {
                order.OrderStatuses = new List<OrderStatus>()
                {
                    new OrderStatus
                    {
                        Status = Domain.Enums.StatusOrder.CONFIRM,
                        Time = DateTime.Now,
                        Note = "Chờ xác nhận"
                    },
                    new OrderStatus
                    {
                        Status = Domain.Enums.StatusOrder.DELIVERIED,
                        Time = DateTime.Now,
                        Note = "Thành công"
                    }
                    //new OrderStatus
                    //{
                    //    Status = Domain.Enums.StatusOrder.DELIVERIED,
                    //    Time = DateTime.Now,
                    //    Note = "Thành công"
                    //},
                };
            }

            order.OrderItems = orderDto.OrderItems.Select(p => new OrderItem
            {
                ProductId = p.ProductId,
                ColorId = p.ColorId,
                SizeId = p.SizeId,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice
            }).ToList();
            
            await _manager.OrderRepository.Post(order);
            #endregion

            #region Update UnitOfStock in stock table
            var StockList = await GetOrderItemsQuantityAsync(order.OrderItems);
            _manager.StockRepository.UpdateRange(StockList);
            Console.WriteLine("Cập nhật số lượng sản phâm thành công.");
            #endregion

            await _manager.UnitOfWork.SaveChangeAsync();
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
               
                #endregion
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public async Task<List<Stock>> GetOrderItemsQuantityAsync(IEnumerable<OrderItem> orderItems)
        {
            var StockList = new List<Stock>();
            foreach (var orderQuantity in orderItems)
            {
                var currentUnitOfStock = await _manager.StockRepository.SelectByVariantId(orderQuantity.ProductId, orderQuantity.ColorId, orderQuantity.SizeId);
                if (currentUnitOfStock == null) throw new System.Exception($"There are something wrong! Could not find the stock with ProducId {orderQuantity.ProductId}, ColorId {orderQuantity.ColorId}, SizeId {orderQuantity.SizeId}.");
                currentUnitOfStock.UnitInStock -= orderQuantity.Quantity;
                StockList.Add(currentUnitOfStock);
            }

            return StockList;
        }

        public async Task<OrderDto> GetByIdOrderAsync(string id, CancellationToken cancellationToken = default)
        {
           var order = await _manager.OrderRepository.GetByIdOrderAsync(id, cancellationToken);

            var orderDto = new OrderDto
            {
                Id = order.Id,
                AddressLine = order.Address,
                PhoneNumber = order.PhoneNumber,
                Note = order.Note,
                Paymethod = order.Paymethod,
                Amount = order.Amount,
                CustomerName = order.CustomerName,
                DateCreated = order.DateCreated,
                PassivedDate = order.PassivedDate,
                ModifiedDate = order.ModifiedDate,
                UserId = order.UserId,
                EmployeeId = order.EmployeeId,
                VoucherId = order.VoucherId,
                OrderStatuses = order.OrderStatuses.Select(p=>new OrderStatusDto
                {
                  
                    OrderId=p.OrderId,
                    Status=p.Status,
                    Time=p.Time,
                    Note=p.Note,
                }).ToList(),
                OrderItems = order.OrderItems.Select(p => new OrderItemDto
                {
                    OrderId = p.OrderId,
                    ProductId = p.ProductId,
                    ColorId = p.ColorId,
                    SizeId  = p.SizeId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                }).ToList()
            };

            return orderDto;
        }

        public async Task<List<OrderByUserIdDto>> GetByUserId(string userId)
        {
            var orderList = await _manager.OrderRepository.SelectByUserId(userId);
          
            return orderList;
        }
    }
}