using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;

namespace Service
{
    public class StockService : IStockService
    {
        private readonly IRepositoryManger _repositoryManger;
        public StockService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }
        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            return await _repositoryManger.StockRepository.GetAllAsync();
        }

        public async Task<Stock> GetStockByIdAsync(string productId, string colorId, string sizeId)
        {
            return await _repositoryManger.StockRepository.SelectById(productId, colorId, sizeId);
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _repositoryManger.StockRepository.AddAsync(stock);
        }

        public async Task UpdateStockRangeAsync(List<Stock> stocks)
        {
            await _repositoryManger.StockRepository.UpdateRange(stocks);
        }

        public async Task DeleteStockAsync(string productId)
        {
            await _repositoryManger.StockRepository.DeleteByProductIdAsync(productId);
            
        }

        // Các phương thức khác tại đây...
    }
}
