using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context=context;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string categoryId)
        {
            var products = await _context.Products
                                         .FromSqlInterpolated($"select * from \"GetByCategory\"({categoryId})")
                                         .ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
