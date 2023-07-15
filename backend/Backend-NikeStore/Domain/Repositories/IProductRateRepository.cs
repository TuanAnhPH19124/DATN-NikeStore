using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRateRepository
    {

        Task AddProductRateAsync(ProductRate productRate);
        Task<ProductRate> GetProductRateAsync(string appUserId, string productId);
        void UpdateProductRate(ProductRate productRate);
        Task<bool> SaveChangesAsync();
    }
}
