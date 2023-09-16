using Domain.DTOs;
using Domain.Entities;
using EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IAddressService
    {
        Task <Address> AddNew(AddressAPI address);
        Task<IEnumerable<AddressDto>> GetByUserId(string id);
        Task Update(AddressUpdateAPI address);
    }
}
