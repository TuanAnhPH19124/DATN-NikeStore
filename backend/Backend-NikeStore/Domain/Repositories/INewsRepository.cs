﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface INewsRepository
    {
        void AddNews(News news);
        Task<List<News>> GetHighlights(int count, CancellationToken cancellationToken = default);
        Task<News> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
