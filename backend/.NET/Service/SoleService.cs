using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service
{
    public class SoleService : ISoleService
    {
        private readonly IRepositoryManger _manager;

        public SoleService(IRepositoryManger manager)
        {
            _manager = manager;
        }
        public async Task<Sole> GetSoleByIdAsync(int id)
        {
            return await _manager.SoleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Sole>> GetAllSolesAsync()
        {
            return await _manager.SoleRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Sole>> FindSolesAsync(Expression<Func<Sole, bool>> predicate)
        {
            return await _manager.SoleRepository.FindAsync(predicate);
        }

        public async Task AddSoleAsync(Sole sole)
        {
            await _manager.SoleRepository.AddAsync(sole);
        }

        public async Task UpdateSoleAsync(Sole sole)
        {
            await _manager.SoleRepository.UpdateAsync(sole);
        }

        public async Task RemoveSoleAsync(Sole sole)
        {
            await _manager.SoleRepository.RemoveAsync(sole);
        }
    }
}
