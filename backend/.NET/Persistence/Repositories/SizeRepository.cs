using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class SizeRepository: ISizeRepository
    {
        private readonly AppDbContext _appDbContext;
        public SizeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<Size>> GetAllSizeAsync(CancellationToken cancellationToken = default)
        {
            var sizeList = await _appDbContext.Sizes.ToListAsync(cancellationToken);
            return sizeList;
        }

        public async Task<Size> GetByIdSizeAsync(string id, CancellationToken cancellationToken = default)
        {
            var size = await _appDbContext.Sizes.FirstOrDefaultAsync(e => e.Id == id);
            return size;
        }

        public async Task AddSize(Size size)
        {

            await _appDbContext.Sizes.AddAsync(size);
            _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateSize(string id, Size size)
        {
            _appDbContext.Sizes.Update(size);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
