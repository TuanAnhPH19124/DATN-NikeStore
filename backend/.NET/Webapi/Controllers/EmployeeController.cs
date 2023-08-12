using Domain.Entities;
using EntitiesDto;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody]Dto.EmployeeDto employees)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Error = ModelState
                });
            }
            var createdEmployee = await _serviceManager.employeeService.CreateAsync(employees);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId }, createdEmployee);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody]Dto.UpdateEmployeeDto employees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    data = employees,
                    error = "Thieu truong thong tin."
                });
            }

            if (id != employees.Id)
                return BadRequest(new { error = "The provided id does not match the id in the user data." });
            

            try
            {
                await _serviceManager.employeeService.UpdateByIdEmployee(id, employees);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ DbUpdateConcurrencyException tại đây
                //return StatusCode((int)HttpStatusCode.Conflict, ex);
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            
        }
    }
}
