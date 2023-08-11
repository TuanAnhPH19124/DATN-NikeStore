using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
        {
            try
            {
                var employee = await _serviceManager.employeeService.GetAllEmployeeAsync();
                if (employee == null || !employee.Any())
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string Id)
        {
            var employee = await _serviceManager.employeeService.GetByIdEmployee(Id);

            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        //[HttpPost]
        //public async Task<ActionResult<Employees>> CreateEmployee(Employees employees)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var createdEmployee = await _serviceManager.employeeService.CreateAsync(employees);
        //        return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId }, createdEmployee);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employees)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Set default role if not provided
                //if (string.IsNullOrEmpty(employees.Role))
                //{
                //    employees.Role = "employee";
                //}

                var createdEmployee = await _serviceManager.employeeService.CreateAsync(employees);
                return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId }, createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, Employee employees)
        {
            if (id != employees.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                await _serviceManager.employeeService.UpdateByIdEmployee(id, employees);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ DbUpdateConcurrencyException tại đây
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
