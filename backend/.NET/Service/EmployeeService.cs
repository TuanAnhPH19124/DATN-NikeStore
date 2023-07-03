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

        public async Task<Employees> CreateAsync(Employees employees)
        {
            _repositoryManger.EmployeeRepository.AddEmployee(employees);

            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return employees;


        }

        public async Task<List<Employees>> GetAllEmployeeAsync(CancellationToken cancellationToken = default)
        {
            List<Employees> employeeList = await _repositoryManger.EmployeeRepository.GetAllEmployeeAsync (cancellationToken);
            return employeeList;
        }

        public async Task<Employees> GetByIdEmployee(Guid id, CancellationToken cancellationToken = default)
        {
            Employees employees = await _repositoryManger.EmployeeRepository.GetByIdAsync(id, cancellationToken);
            return employees;
        }

        public async Task<Employees> UpdateByIdEmployee(Guid id, Employees employees, CancellationToken cancellationToken = default)
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
                existingEmployee.Password= employees.Password;
                existingEmployee.Role= employees.Role;
                existingEmployee.Status = employees.Status;              
                await _repositoryManger.UnitOfWork.SaveChangeAsync(cancellationToken);
                return existingEmployee;
            }
        }       
    }
}
