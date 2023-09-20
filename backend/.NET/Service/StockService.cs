using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Stock;
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

        public async Task<IEnumerable<Stock>> GetStockByIdAsync(string productId)
        {
            return await _repositoryManger.StockRepository.SelectById(productId);
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _repositoryManger.StockRepository.AddAsync(stock);
        }

        public void UpdateStockRangeAsync(List<Stock> stocks)
        {
            _repositoryManger.StockRepository.UpdateRange(stocks);
        }

        public async Task DeleteStockAsync(string productId)
        {
            await _repositoryManger.StockRepository.DeleteByProductIdAsync(productId);
            
        }

        public async Task<string> GetStockIdList(GetStockIdAPI item)
        {
            var result = await _repositoryManger.StockRepository.GetStockId(item.ProductId, item.ColorId, item.SizeId);
            return result;
        }

        public async Task<Stock> GetStockByRelation(string productId, string colorId, string sizeId)
        {
            var result = await _repositoryManger.StockRepository.SelectByVariantId(productId, colorId, sizeId);
            return result;
        }


        // Các phương thức khác tại đây...
    }
}
