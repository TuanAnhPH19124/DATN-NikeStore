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
        void AddEmployee(Employee employees);
        void UpdateEmployee(Employee employees);
    }
}
