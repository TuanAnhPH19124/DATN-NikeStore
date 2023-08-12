using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManger _repositoryManger;

        public EmployeeService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }
        public async Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default)
        {
            List<Employee> employeeList = await _repositoryManger.EmployeeRepository.GetAllEmployeeAsync(cancellationToken);
            return employeeList;
        }

        public async Task<Employee> GetByIdEmployeeAsync(string id, CancellationToken cancellationToken = default)
        {
            Employee employee = await _repositoryManger.EmployeeRepository.GetByIdEmployeeAsync(id, cancellationToken);
            return employee;
        }
        public async Task<Employee> CreateAsync(Employee employee)
        {
            await _repositoryManger.EmployeeRepository.AddEmployee(employee);
            // await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return employee;
        }
        public async Task<Employee> UpdateByIdEmployee(string id, Employee employee, CancellationToken cancellationToken = default)
        {
            var existingEmployee = await _repositoryManger.EmployeeRepository.GetByIdEmployeeAsync(id, cancellationToken);
            if (existingEmployee == null)
            {
                throw new Exception("User not found.");
            }
            else
            {
                existingEmployee.DateOfBirth = employee.DateOfBirth;
                existingEmployee.FullName= employee.FullName;
                existingEmployee.SNN = employee.SNN;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.Address= employee.Address;               
                await _repositoryManger.EmployeeRepository.UpdateEmployee(id, existingEmployee);
                return existingEmployee;
            }
        }
    }
}
