using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IProductRateService
    {

        Task<ProductRate> AddProductRateAsync(ProductRate productRate);
        Task<ProductRate> GetProductRateAsync(string appUserId, string productId);
        Task UpdateProductRate(ProductRate productRate);
        Task<bool> SaveChangesAsync();
    }
}
