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
    internal sealed class ColorService : IColorService
    {
        private readonly IRepositoryManger _repositoryManger;

        public ColorService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }
        public async Task<List<Color>> GetAllColorAsync(CancellationToken cancellationToken = default)
        {
            List<Color> colorList = await _repositoryManger.ColorRepository.GetAllColorAsync(cancellationToken);
            return colorList;
        }

        public async Task<Color> GetByIdColorAsync(string id, CancellationToken cancellationToken = default)
        {
            Color color = await _repositoryManger.ColorRepository.GetByIdColorAsync(id, cancellationToken);
            return color;
        }
        public async Task<Color> CreateAsync(Color color)
        {
            await _repositoryManger.ColorRepository.AddColor(color);
            // await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return color;
        }
        public async Task<Color> UpdateByIdColor(string id, Color color, CancellationToken cancellationToken = default)
        {
            var existingColor = await _repositoryManger.ColorRepository.GetByIdColorAsync(id, cancellationToken);
            if (existingColor == null)
            {
                throw new Exception("User not found.");
            }
            else
            {
                existingColor.Name = color.Name;             
                await _repositoryManger.ColorRepository.UpdateColor(id, existingColor);
                return existingColor;
            }
        }
    }
}
