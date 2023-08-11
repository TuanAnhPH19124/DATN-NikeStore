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
        Task<Employee> CreateAsync(Employee employees);
        Task<Employee> UpdateByIdEmployee(string id, Employee employees, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default);
        Task<Employee> GetByIdEmployee(string id, CancellationToken cancellationToken = default);
    }
}
