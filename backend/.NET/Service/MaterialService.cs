using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MaterialService : IMaterialService
    {
        private readonly IRepositoryManger _repositoryManger;

        public MaterialService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        public async Task<Material> GetMaterialByIdAsync(int id)
        {
            return await _repositoryManger.MaterialRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
        {
            return await _repositoryManger.MaterialRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Material>> FindMaterialsAsync(Expression<Func<Material, bool>> predicate)
        {
            return await _repositoryManger.MaterialRepository.FindAsync(predicate);
        }

        public async Task AddMaterialAsync(Material material)
        {
            await _repositoryManger.MaterialRepository.AddAsync(material);
        }

        public async Task UpdateMaterialAsync(Material material)
        {
            await _repositoryManger.MaterialRepository.UpdateAsync(material);
        }

        public async Task RemoveMaterialAsync(Material material)
        {
            await _repositoryManger.MaterialRepository.RemoveAsync(material);
        }
    }
}
