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
        public async Task AddAsync( OrderStatus orderStatus)
        {

           await _repositoryManger.OrderStatusRepository.AddAsync(orderStatus);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
        }
    }
}
