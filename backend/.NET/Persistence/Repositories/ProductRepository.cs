using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Product;
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
                return await _context.Products
                    .Include(p => p.Stocks)
                    .Include(p => p.CategoryProducts)
                    .Include(p=>p.ProductImages)
                    .ToListAsync();
        }

        
        public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
            .Include(p => p.Stocks)
            .Include(p => p.CategoryProducts)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<List<Product>> FilterProductsAsync(
        string sizeId, string colorId, string categoryId, int? materialId, int? soleId)
        {
            var allProducts = await _context.Products
                .Include(p => p.Stocks)
                .Include(p => p.CategoryProducts)
                .ToListAsync();

            // Lọc sản phẩm theo các tham số

            // Lọc theo kích thước (size)
            if (!string.IsNullOrEmpty(sizeId))
            {
                allProducts = allProducts.Where(product =>
                    product.Stocks.Any(stock => stock.SizeId == sizeId)).ToList();
            }

            // Lọc theo màu sắc (color)
            if (!string.IsNullOrEmpty(colorId))
            {
                allProducts = allProducts.Where(product =>
                    product.Stocks.Any(stock => stock.ColorId == colorId)).ToList();
            }

            // Lọc theo danh mục (category)
            if (!string.IsNullOrEmpty(categoryId))
            {
                allProducts = allProducts.Where(product =>
                    product.CategoryProducts.Any(categoryProduct => categoryProduct.CategoryId == categoryId)).ToList();
            }

            // Lọc theo chất liệu (material)
            if (materialId.HasValue)
            {
                allProducts = allProducts.Where(product =>
                    product.MaterialId == materialId).ToList();
            }

            // Lọc theo đế giày (sole)
            if (soleId.HasValue)
            {
                allProducts = allProducts.Where(product =>
                    product.SoleId == soleId).ToList();
            }

            return allProducts;
        }

        public async Task<IEnumerable<Product>> GetProductByFilterAndSort(IQueryable<Product> query)
        {
            return await query.ToListAsync();
        }

        public IQueryable<Product> GetAllProductsQuery()
        {
            return _context.Products.Include(p => p.CategoryProducts).Include(p => p.ProductImages).Include(p => p.Stocks).AsQueryable();
        }
    }
}
