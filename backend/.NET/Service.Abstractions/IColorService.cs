using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IColorService
    {
        Task<List<Color>> GetAllColorAsync(CancellationToken cancellationToken = default);
        Task<Color> GetByIdColorAsync(string id, CancellationToken cancellationToken = default);
        Task<Color> CreateAsync(Color color);
        Task<Color> UpdateByIdColor(string id, Color color, CancellationToken cancellationToken = default);

    }
}
