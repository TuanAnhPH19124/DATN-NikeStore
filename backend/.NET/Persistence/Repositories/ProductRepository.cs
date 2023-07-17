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
        } 

        public async Task<List<Product>> GetAllProductAsync(CancellationToken cancellationToken = default)
        {
            List<Product> productList = await _context.Products.ToListAsync(cancellationToken);
            return productList;
        }
        public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == id );
            return product;
        }

        public async void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
