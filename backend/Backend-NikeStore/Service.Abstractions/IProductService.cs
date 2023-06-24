using Domain.Entities;
using EntitiesDto.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductForThirdServiceDto>> SelectByCategoryOnCacheAsync(string categoryId);
        Task<IEnumerable<ProductForThirdServiceDto>> SelectProductOnCacheAsync();
    }
}
