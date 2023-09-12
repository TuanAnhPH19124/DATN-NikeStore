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

        public async Task<List<string>> GetStockIdList(List<GetStockIdAPI> items)
        {
            var task = await Task.Run(() => {
                var result = new List<string>();
                items.ForEach(async item =>{
                    var stockId = await _repositoryManger.StockRepository.GetStockId(item.ProductId, item.ColorId, item.SizeId);
                    result.Add(stockId);
                });
                return result;
            });
            
            return task;
        }


        // Các phương thức khác tại đây...
    }
}
