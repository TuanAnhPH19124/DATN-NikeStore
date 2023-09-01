using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class AddressService : IAddressService
    {
        private readonly IRepositoryManger _repositoryManger;

        public AddressService(IRepositoryManger repositoryManger)
        {
            _repositoryManger=repositoryManger;
        }

        public async Task AddNew(AddressAPI address)
        {
            var newAddress = address.Adapt<Address>();
          
            await _repositoryManger.AddressRepository.Add(newAddress);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
        }
    }
}
