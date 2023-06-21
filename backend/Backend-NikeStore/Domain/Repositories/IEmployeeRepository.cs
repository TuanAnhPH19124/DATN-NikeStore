using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employees>> GetAllEmployeeAsync(CancellationToken cancellationToken = default);
        Task<Employees> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void AddEmployee(Employees employees);
        void UpdateEmployee(Employees employees);
    }
}
