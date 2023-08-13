using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;
using EntitiesDto.Order;

namespace Service.Abstractions
{
    public interface IOrderService
    {
        Task PostAndSendNontification(OrderPostRequestDto orderDto);

        Task UpdateOrderOnConfirm(string id ,object order);
        Task PostNewOrderAtStore(OrderAtStorePostRequestDto orderDto);
        Task<List<OrderDto>> GetAllOrderAsync(CancellationToken cancellationToken = default);
        Task<Order> GetByIdOrderAsync(string id, CancellationToken cancellationToken = default);
    }
}