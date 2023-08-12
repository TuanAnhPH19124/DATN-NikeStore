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
        Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default);
        Task<Employee> GetByIdEmployeeAsync(string id, CancellationToken cancellationToken = default);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateByIdEmployee(string id, Employee employee, CancellationToken cancellationToken = default);
    }
}
