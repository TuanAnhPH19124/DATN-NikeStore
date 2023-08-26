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
        Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default);
        Task<Employee> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task AddEmployee(Employee employees);
        void UpdateEmployee(string id, Employee employees);
    }
}
