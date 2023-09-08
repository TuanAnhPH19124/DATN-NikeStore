using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Service.Abstractions
{
    public interface ISizeService
    {
        Task<List<Domain.Entities.Size>> GetSizeForProduct(string productId, string colorId);

        Task<List<Domain.Entities.Size>> GetAllSizeAsync(CancellationToken cancellationToken = default);
        Task<Domain.Entities.Size> GetByIdSizeAsync(string id, CancellationToken cancellationToken = default);
        Task<Domain.Entities.Size> GetByNumberSizeAsync(int numberSize, CancellationToken cancellationToken = default);
        Task<Domain.Entities.Size> CreateAsync(Domain.Entities.Size size);
        Task<Domain.Entities.Size> UpdateByIdSize(string id, Domain.Entities.Size size, CancellationToken cancellationToken = default);
    }
}
