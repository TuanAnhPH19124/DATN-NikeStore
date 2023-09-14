using Domain.DTOs;
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

        public async Task <Address> AddNew(AddressAPI address)
        {
            var newAddress = address.Adapt<Address>();
            if (address.SetAsDefault){
                var addresses = await _repositoryManger.AddressRepository.GetByUserId(address.UserId);
                foreach (var item in addresses)
                {
                    item.SetAsDefault = false;
                }
                _repositoryManger.AddressRepository.UpdateRange(addresses.ToList());
            }
            await _repositoryManger.AddressRepository.Add(newAddress);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return newAddress;
        }

        public async Task<IEnumerable<AddressDto>> GetByUserId(string id)
        {
            var addresses = await _repositoryManger.AddressRepository.GetByUserId(id);

            var addressesDto = addresses.Select(p => new AddressDto{
                Id = p.Id,
                AddressLine = p.AddressLine,
                PhoneNumber = p.PhoneNumber,
                CityCode = p.CityCode,
                ProvinceCode = p.ProvinceCode,
                WardCode = p.WardCode,
                FullName = p.FullName,
                SetAsDefault = p.SetAsDefault
            }).ToList();

            return addressesDto;
        }
    }
}
