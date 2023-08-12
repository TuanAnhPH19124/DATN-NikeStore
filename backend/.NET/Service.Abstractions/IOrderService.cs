using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using EntitiesDto.Order;

namespace Service.Abstractions
{
    public interface IOrderService
    {
        Task PostAndSendNontification(OrderPostRequestDto orderDto);

        Task UpdateOrderOnConfirm(string id ,object order);
        Task PostNewOrderAtStore(OrderAtStorePostRequestDto orderDto);
        Task<List<Order>> GetAllOrderAsync(CancellationToken cancellationToken = default);
    }
}