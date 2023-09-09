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

        // Doanh so trong 1 khoang thoi gian
        public async Task<float> GetTotalOrder(DateTime startDate, DateTime endDate)
        {
            try
            {
                var summary = await _repositoryManger.OrderItemsRepository.GetTotalOrders(startDate, endDate);
                return summary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // doanh so ban hang cua thang hien tai
        public async Task<float> GetTotalOrdersForCurrentMonth()
        {
            try
            {
                var sumAllMonth = await _repositoryManger.OrderItemsRepository.GetTotalOrdersForCurrentMonth();
                return sumAllMonth;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // doanh so ban hang cua ngay hom nay
        public async Task<float> GetTotalOrdersForCurrentDate()
        {
            try
            {
                var sumAllDate = await _repositoryManger.OrderItemsRepository.GetTotalOrdersForCurrentDate();
                return sumAllDate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Tong so don hang cua ngay hom nay
        public async Task<float> GetTotalBillForCurrentDate()
        {
            try
            {
                var sumAllDate = await _repositoryManger.OrderItemsRepository.GetTotalBillForCurrentDate();
                return sumAllDate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Tong so don hang cua thang hien tai
        public async Task<float> GetTotalBillForCurrentMonth()
        {
            try
            {
                var sumAllMonth = await _repositoryManger.OrderItemsRepository.GetTotalBillForCurrentMonth();
                return sumAllMonth;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Tong so hoa don trong 1 khoang thoi gian
        public async Task<float> GetTotalOrdersInTimeRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var sumAll = await _repositoryManger.OrderItemsRepository.GetTotalOrdersInTimeRange(startDate, endDate);
                return sumAll;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Tong so hoa don da ban
        public async Task<float> GetTotalBill()
        {
            try
            {
                var sumAllBill = await _repositoryManger.OrderItemsRepository.GetTotalBill();
                return sumAllBill;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Tong so doanh thu
        public async Task<float> GetTotalAmount()
        {
            try
            {
                var sumAllOrder = await _repositoryManger.OrderItemsRepository.GetTotalAmount();
                return sumAllOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
