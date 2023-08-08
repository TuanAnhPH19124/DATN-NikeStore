using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IStockRepository
    {
        Task UpdateRange(List<Stock> Stocks);
        Task<Stock> SelectById(string productId);
        Task<IEnumerable<Stock>> GetAllAsync();
        Task AddAsync(Stock stock);
        Task DeleteByProductIdAsync(string productId);
    }
}