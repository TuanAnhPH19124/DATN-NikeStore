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

        public async Task<Employee> CreateAsync(Employee employees)
        {
            _repositoryManger.EmployeeRepository.AddEmployee(employees);

            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return employees;


        }

        public async Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default)
        {
            List<Employee> employeeList = await _repositoryManger.EmployeeRepository.GetAllEmployeeAsync (cancellationToken);
            return employeeList;
        }

        public async Task<Employee> GetByIdEmployee(string id, CancellationToken cancellationToken = default)
        {
            Employee employees = await _repositoryManger.EmployeeRepository.GetByIdAsync(id, cancellationToken);
            return employees;
        }

        public async Task<Employee> UpdateByIdEmployee(string id, Employee employees, CancellationToken cancellationToken = default)
        {
            var existingEmployee = await _repositoryManger.EmployeeRepository.GetByIdAsync(id, cancellationToken);
            if (existingEmployee == null)
            {
                throw new Exception("Employee not found.");
            }
            else
            {
                existingEmployee.SNN = employees.SNN;
                existingEmployee.FullName= employees.FullName;
                existingEmployee.PhoneNumber= employees.PhoneNumber;
                existingEmployee.Status = employees.Status;              
                await _repositoryManger.UnitOfWork.SaveChangeAsync(cancellationToken);
                return existingEmployee;
            }
        }       
    }
}
