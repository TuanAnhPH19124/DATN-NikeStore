using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAddressRepository
    {
        Task Add(Address address);
        Task<IEnumerable<Address>> GetByUserId(string id);
        void UpdateRange(List<Address> addresses);
        void Update(string id, Address address);
    }
}
