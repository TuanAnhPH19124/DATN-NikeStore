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

        public async void AddNews(News news)
        {
            _dbContext.News.Add(news);
        }

        public async Task<News> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var news = await _dbContext.News.FirstOrDefaultAsync(c => c.Id == id);
            return news;
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

