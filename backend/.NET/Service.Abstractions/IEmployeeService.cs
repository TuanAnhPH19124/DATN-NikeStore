using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IEmployeeService
    {
        Task<Employees> CreateAsync(Employees employees);
        Task<Employees> UpdateByIdEmployee(Guid id, Employees employees, CancellationToken cancellationToken = default);
        Task<List<Employees>> GetAllEmployeeAsync(CancellationToken cancellationToken = default);
        Task<Employees> GetByIdEmployee(Guid id, CancellationToken cancellationToken = default);
    }
}
