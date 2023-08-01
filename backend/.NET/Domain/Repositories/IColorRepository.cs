using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IColorRepository
    {
        Task<List<Color>> GetAllColorAsync(CancellationToken cancellationToken = default);
        Task<Color> GetByIdColorAsync(string id, CancellationToken cancellationToken = default);
        Task AddColor(Color color);
        Task UpdateColor(string id, Color color);
    }
}
