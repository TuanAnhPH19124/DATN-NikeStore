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
    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context=context;
        }

        public async Task AddAsync(Category category)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Categories.AddAsync(category);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
         
        }

        public Task<bool> FindByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            var categories = await _context.Categories.Select(c => new {c.Name, c.Id}).ToListAsync();
            return categories;
        }

        public async Task<object> GetByIdAsync(string id)
        {
             return await _context.Categories.FindAsync(id);
        }

        public async Task UpdateAsync(string id, Category category)
        {
            var targetC = await _context.Categories.FindAsync(id);
            if (targetC != null)
            {
                targetC.Name = category.Name;
                targetC.ModifiedDate = DateTime.Now;
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Categories.Update(targetC);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task DeleteAsync(string id)
        {
            var categoryToDelete = await _context.Categories.FindAsync(id);

            if (categoryToDelete != null)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Categories.Remove(categoryToDelete);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            else
            {
                // Throw an exception or handle the case when the category is not found
                throw new Exception("Category not found.");
            }
        }


    }
}
