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
    public class CategoryProductRepository:ICategoryProductRepository
    {
        private readonly AppDbContext _context;

        public CategoryProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryProduct> GetByIdAsync(string productId, string categoryId)
        {
            return await _context.CategoryProducts
                .FirstOrDefaultAsync(cp => cp.ProductId == productId && cp.CategoryId == categoryId);
        }

        public async Task AddAsync(CategoryProduct categoryProduct)
        {
            await _context.CategoryProducts.AddAsync(categoryProduct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryProduct categoryProduct)
        {
            _context.CategoryProducts.Update(categoryProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string productId, string categoryId)
        {
            var categoryProduct = await GetByIdAsync(productId, categoryId);
            if (categoryProduct != null)
            {
                _context.CategoryProducts.Remove(categoryProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryProduct>> GetAllAsync()
        {
            return await _context.CategoryProducts.ToListAsync();
        }
    }
}
