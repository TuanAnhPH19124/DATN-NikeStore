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
        Task<Stock> SelectById(string productId, string colorId, string sizeId);
    }
}