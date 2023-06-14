﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IProductService
    {
        Task<Product> CreateAsync(Product product);
        Task<List<Product>> GetAllProductAsync(CancellationToken cancellationToken = default);
        Task<Product> GetByIdProduct(string id, CancellationToken cancellationToken = default);
        Task<Product> UpdateByIdProduct(string id, Product product, CancellationToken cancellationToken = default);
    }
}
