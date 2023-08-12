using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;
        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default)
        {
            List<Employee> employeeList = await _appDbContext.Employees.ToListAsync(cancellationToken);
            return employeeList;
        }
        public async Task<Employee> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var employee = await _appDbContext.Employees .FirstOrDefaultAsync(e => e.Id == id);
            return employee;
        }

        public async void AddEmployee (Employee employees)
        {
            _appDbContext.Employees.Add(employees);
        }

        public async void UpdateEmployee(Employee employees)
        {
            _appDbContext.Employees.Update(employees);
        }
    }
}
