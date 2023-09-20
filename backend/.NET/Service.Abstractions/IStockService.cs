using Domain.Entities;
using EntitiesDto.Stock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IStockService
    {
        Task<Stock> GetStockByRelation(string productId, string colorId, string sizeId);
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<IEnumerable<Stock>> GetStockByIdAsync(string productId);
        Task AddStockAsync(Stock stock);
        void UpdateStockRangeAsync(List<Stock> stocks);
        Task DeleteStockAsync(string productId);
        Task<string> GetStockIdList(GetStockIdAPI item);
    }
}
