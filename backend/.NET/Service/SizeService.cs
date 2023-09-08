using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class SizeService : ISizeService
    {
        private readonly IRepositoryManger _repositoryManger;

        public SizeService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }
        public async Task<List<Size>> GetAllSizeAsync(CancellationToken cancellationToken = default)
        {
            List<Size> sizeList = await _repositoryManger.SizeRepository.GetAllSizeAsync(cancellationToken);
            return sizeList;
        }

        public async Task<Size> GetByIdSizeAsync(string id, CancellationToken cancellationToken = default)
        {
            Size size = await _repositoryManger.SizeRepository.GetByIdSizeAsync(id, cancellationToken);
            return size;
        }

        public async Task<Size> GetByNumberSizeAsync(int numberSize, CancellationToken cancellationToken = default)
        {
            return await _repositoryManger.SizeRepository.GetByNumberSizeAsync(numberSize, cancellationToken);
        }
        public async Task<Size> CreateAsync(Size size)
        {
            await _repositoryManger.SizeRepository.AddSize(size);
           // await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return size;
        }

        public async Task<Size> UpdateByIdSize(string id, Size size, CancellationToken cancellationToken = default)
        {
            var existingSize = await _repositoryManger.SizeRepository.GetByIdSizeAsync(id, cancellationToken);
            if (existingSize == null)
            {
                throw new Exception("User not found.");
            }
            else
            {
                existingSize.NumberSize = size.NumberSize;
                existingSize.Description = size.Description;
                await _repositoryManger.SizeRepository.UpdateSize(id, existingSize);
                return existingSize;
            }
        }

        public async Task<List<Size>> GetSizeForProduct(string productId, string colorId)
        {
            var sizes = await _repositoryManger.SizeRepository.GetSizeForProduct(productId, colorId);
            return sizes;
        }

    }
}
