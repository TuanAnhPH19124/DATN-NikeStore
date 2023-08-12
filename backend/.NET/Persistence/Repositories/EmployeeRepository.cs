using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
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
            var employee = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            return employee;
        }

        public async Task AddEmployee (Employee employees)
        {
            using (var transaction = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    await _appDbContext.Employees.AddAsync(employees);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async void UpdateEmployee(string id, Employee employees)
        {
            var emp = _appDbContext.Employees.Find(id);

            if (emp == null)
                throw new Exception("Khong tim thay nhan vien nay");

            emp.ModifiedDate = employees.ModifiedDate;
            emp.SNN = employees.SNN;
            emp.PhoneNumber = employees.PhoneNumber;
            emp.FullName = employees.FullName;
            emp.DateOfBirth = employees.DateOfBirth;
            emp.Gender = employees.Gender;
            emp.HomeTown = employees.HomeTown;
            emp.Address = employees.Address;
            emp.RelativeName = employees.RelativeName;
            emp.RelativePhoneNumber = employees.RelativePhoneNumber;
            emp.Status = emp.Status;

            using (var transaction = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    _appDbContext.Employees.Update(emp);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
