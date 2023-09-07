using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _dbContext;

        public AddressRepository(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task Add(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
        }

        public async Task<IEnumerable<Address>> GetByUserId(string id)
        {
            return await _dbContext.Addresses.Where(p => p.UserId == id).ToListAsync();
        }

        public void Update(string id, Address address)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(List<Address> addresses)
        {
            _dbContext.Addresses.UpdateRange(addresses);
        }
    }
}
