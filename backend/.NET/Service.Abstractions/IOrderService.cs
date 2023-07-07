using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using EntitiesDto.Order;

namespace Service.Abstractions
{
    public interface IOrderService
    {
        Task PostAndSendNontification(OrderPostRequestDto order);

        Task UpdateOrderOnConfirm(string id ,object order);
    }
}