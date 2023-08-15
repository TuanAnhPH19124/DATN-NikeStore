using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IMaterialService
    {
        Task<Material> GetMaterialByIdAsync(int id);
        Task<IEnumerable<Material>> GetAllMaterialsAsync();
        Task<IEnumerable<Material>> FindMaterialsAsync(Expression<Func<Material, bool>> predicate);
        Task AddMaterialAsync(Material material);
        Task UpdateMaterialAsync(Material material);
        Task RemoveMaterialAsync(Material material);
    }
}
