using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class CategoryService : ICategoryService
    {
        private readonly IRepositoryManger _repositoryManger;

        public CategoryService(IRepositoryManger repositoryManger)
        {
            _repositoryManger=repositoryManger;
        }

        public async Task<IEnumerable<object>> SelectAllCategoriesOnCache()
        {
            var cacheData = _repositoryManger.CacheRepository.GetData<IEnumerable<object>>("categories");
            if (cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }
            cacheData = await _repositoryManger.CategoryRepository.GetAllAsync();
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            _repositoryManger.CacheRepository.SetData<IEnumerable<object>>("categories", cacheData, expiryTime);
            return cacheData;
        }
    }
}
