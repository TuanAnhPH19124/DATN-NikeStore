using Domain.Repositories;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Service.Abstractions;

namespace Service
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IRepositoryManger _repositoryManger;

        public OrderItemService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        // So luong ban cua 1 sp
        public async Task<int> GetAmountByProductId(string productId)
        {
            return await _repositoryManger.OrderItemsRepository.GetAmountByProductId(productId);
        }

        // thong ke doanh thu cua 1 sp
        public async Task<decimal> GetRevenueByProductId(string productId)
        {
            return await _repositoryManger.OrderItemsRepository.GetRevenueByProductId(productId);
        }

        // thong ke tong doanh thu
        public async Task<decimal> GetTotalRevenueInTimeRange(DateTime startDate, DateTime endDate)
        {
            return await _repositoryManger.OrderItemsRepository.GetTotalRevenueInTimeRange(startDate, endDate);
        }
    }
}
