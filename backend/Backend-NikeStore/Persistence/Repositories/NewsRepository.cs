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
   
        public class NewsRepository : INewsRepository
        {
            private readonly AppDbContext _dbContext;

            public NewsRepository(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<News>> GetHighlights(int count, CancellationToken cancellationToken = default)
            {
               return await _dbContext.Set<News>()
               .OrderByDescending(n => n.CreatedAt)
               .Take(count)
               .ToListAsync(cancellationToken);
            }
        }

}

