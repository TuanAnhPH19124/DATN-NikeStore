using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Product>> GetAllProductAsync(CancellationToken cancellationToken = default)
        {
            List<Product> productList = await _appDbContext.Products.ToListAsync(cancellationToken);
            return productList;
        }
        public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(e => e.Id == id );
            return product;
        }

        public async void AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
        }

        public async void UpdateProduct(Product product)
        {
            _appDbContext.Products.Update(product);
        }
    }
}
