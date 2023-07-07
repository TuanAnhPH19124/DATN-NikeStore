﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string categoryId);
        Task<IEnumerable<Product>> GetProductAsync();
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        Task<List<Product>> GetAllProductAsync(CancellationToken cancellationToken = default);
        Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}