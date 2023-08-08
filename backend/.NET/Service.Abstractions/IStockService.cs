using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> GetStockByIdAsync(string productId);
        Task AddStockAsync(Stock stock);
        Task UpdateStockRangeAsync(List<Stock> stocks);
        Task DeleteStockAsync(string productId);
    }
}
