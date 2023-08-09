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
    internal sealed class ColorRepository : IColorRepository
    {
        private readonly AppDbContext _appDbContext;
        public ColorRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<Color>> GetAllColorAsync(CancellationToken cancellationToken = default)
        {
            var colorList = await _appDbContext.Colors.ToListAsync(cancellationToken);
            return colorList;
        }

        public async Task<Color> GetByIdColorAsync(string id, CancellationToken cancellationToken = default)
        {
            var color = await _appDbContext.Colors.FirstOrDefaultAsync(e => e.Id == id);
            return color;
        }

        public async Task AddColor(Color color)
        {
            await _appDbContext.Colors.AddAsync(color);
            _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateColor(string id, Color color)
        {
            _appDbContext.Colors.Update(color);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
