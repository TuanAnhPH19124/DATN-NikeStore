using Domain.Entities;
using Domain.Repositories;
using EntitiesDto;
using Mapster;
using Service.Abstractions;
using System;
using System.Collections.Generic;
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

        public async Task<Employee> CreateAsync(Dto.EmployeeDto employees)
        {
            var newEmployee = employees.Adapt<Employee>();
            await _repositoryManger.EmployeeRepository.AddEmployee(newEmployee);
            await _repositoryManger.UnitOfWork.SaveChangeAsync();
            return newEmployee;
        }

        public async Task<List<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken = default)
        {
            List<Employee> employeeList = await _repositoryManger.EmployeeRepository.GetAllEmployeeAsync(cancellationToken);
            return employeeList;
        }

        public async Task<Employee> GetByIdEmployee(string id, CancellationToken cancellationToken = default)
        {
            Employee employees = await _repositoryManger.EmployeeRepository.GetByIdAsync(id, cancellationToken);
            return employees;
        }

        public async Task UpdateByIdEmployee(string id, Dto.UpdateEmployeeDto employees, CancellationToken cancellationToken = default)
        {
            try
            {
                var updateEmp = employees.Adapt<Employee>();
                _repositoryManger.EmployeeRepository.UpdateEmployee(id, updateEmp);
                await _repositoryManger.UnitOfWork.SaveChangeAsync(cancellationToken);

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
