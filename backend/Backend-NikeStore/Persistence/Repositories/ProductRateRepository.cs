using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ProductRateRepository : IProductRateRepository
    {
        private readonly AppDbContext _context;
        public ProductRateRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task AddProductRateAsync(ProductRate productRate)
        {
            await _context.ProductRate.AddAsync(productRate);
        }

        public async Task<ProductRate> GetProductRateAsync(string appUserId, string productId)
        {
            return await _context.ProductRate.FindAsync(appUserId, productId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProductRate(ProductRate productRate)
        {
            _context.ProductRate.Update(productRate);
        }
    }
}
