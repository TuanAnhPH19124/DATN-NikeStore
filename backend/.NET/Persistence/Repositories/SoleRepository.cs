using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class SoleRepository : ISoleRepository
    {
        private readonly AppDbContext  _context;

        public SoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sole> GetByIdAsync(int id)
        {
            return await _context.Soles.FindAsync(id);
        }

        public async Task<IEnumerable<Sole>> GetAllAsync()
        {
            return await _context.Soles.ToListAsync();
        }

        public async Task<IEnumerable<Sole>> FindAsync(Expression<Func<Sole, bool>> predicate)
        {
            return await _context.Soles.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(Sole sole)
        {
            await _context.Soles.AddAsync(sole);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sole sole)
        {
            _context.Soles.Update(sole);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Sole sole)
        {
            _context.Soles.Remove(sole);
            await _context.SaveChangesAsync();
        }
    }
}
