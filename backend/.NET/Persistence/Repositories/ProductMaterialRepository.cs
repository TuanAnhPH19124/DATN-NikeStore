using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Persistence.Repositories
{
    public class ProductMaterialRepository : IProductMaterialRepository
    {
        private readonly AppDbContext _context;

        public ProductMaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductMaterial> GetAsync(int materialId, string productId)
        {
            return await _context.ProductMaterials
                .Include(pm => pm.Product)
                .Include(pm => pm.Material)
                .FirstOrDefaultAsync(pm => pm.MaterialId == materialId && pm.ProductId == productId);
        }

        public async Task<IEnumerable<ProductMaterial>> GetAllAsync()
        {
            return await _context.ProductMaterials
                .Include(pm => pm.Product)
                .Include(pm => pm.Material)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductMaterial>> FindAsync(Expression<Func<ProductMaterial, bool>> predicate)
        {
            return await _context.ProductMaterials
                .Include(pm => pm.Product)
                .Include(pm => pm.Material)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task AddAsync(ProductMaterial entity)
        {
            await _context.ProductMaterials.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductMaterial entity)
        {
            _context.ProductMaterials.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ProductMaterial entity)
        {
            _context.ProductMaterials.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetMaterialIdForProductAsync(string productId)
        {
            var productMaterial = await _context.ProductMaterials
              .Where(pm => pm.ProductId == productId)
              .FirstOrDefaultAsync();

            return productMaterial?.MaterialId;
        }
    }
}
