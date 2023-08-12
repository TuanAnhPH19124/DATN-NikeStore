using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;
using EntitiesDto;
using Mapster;

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

        [HttpGet("Get")]
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

        [HttpGet("Get/{Id}")]
        public async Task<ActionResult<Employee>> GetByIdEmployee(string Id)
        {
            var employee = await _serviceManager.employeeService.GetByIdEmployeeAsync(Id);

            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto)
        {
            try
            {
                var employee = employeeDto.Adapt<Employee>();
                await _serviceManager.employeeService.CreateAsync(employee);
                return CreatedAtAction(nameof(GetByIdEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, EmployeeUpdateDto employeeUpdateDto)
        {
            if (id != employeeUpdateDto.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                var employee = employeeUpdateDto.Adapt<Employee>();
                await _serviceManager.employeeService.UpdateByIdEmployee(id, employee);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
