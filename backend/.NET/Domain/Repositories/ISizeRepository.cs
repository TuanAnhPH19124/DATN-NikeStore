using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ISizeRepository
    {
        Task<List<Size>> GetAllSizeAsync(CancellationToken cancellationToken = default);
        Task<Size> GetByIdSizeAsync(string id, CancellationToken cancellationToken = default);
        Task<Size> GetByNumberSizeAsync(int numberSize, CancellationToken cancellationToken = default);
        Task AddSize(Size size);
        Task UpdateSize(string id, Size size);
    }
}
