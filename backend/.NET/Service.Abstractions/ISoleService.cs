using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;

namespace Service.Abstractions
{
    public interface ISoleService
    {
        Task<Sole> GetSoleByIdAsync(int id);
        Task<IEnumerable<Sole>> GetAllSolesAsync();
        Task<IEnumerable<Sole>> FindSolesAsync(Expression<Func<Sole, bool>> predicate);
        Task AddSoleAsync(Sole sole);
        Task UpdateSoleAsync(Sole sole);
        Task RemoveSoleAsync(Sole sole);
    }
}
