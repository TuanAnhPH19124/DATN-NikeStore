using Domain.Entities;
using EntitiesDto;
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
        Task<Employee> CreateAsync(Dto.EmployeeDto employees);
        Task UpdateByIdEmployee(string id, Dto.UpdateEmployeeDto employees, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default);
        Task<Employee> GetByIdEmployee(string id, CancellationToken cancellationToken = default);
    }
}
