using Domain.Entities;
using EntitiesDto;
using EntitiesDto.Sole;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoleController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SoleController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSole(int id)
        {
            var sole = await _serviceManager.SoleService.GetSoleByIdAsync(id);
            if (sole == null)
            {
                return NotFound();
            }

            return Ok(sole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSoles()
        {
            var soles = await _serviceManager.SoleService.GetAllSolesAsync();
            return Ok(soles);
        }

        [HttpGet("search")]
        public async Task<IActionResult> FindSoles([FromQuery] string keyword)
        {
            Expression<Func<Sole, bool>> predicate = s => s.Name.Contains(keyword) || s.Description.Contains(keyword);
            var matchingSoles = await _serviceManager.SoleService.FindSolesAsync(predicate);
            return Ok(matchingSoles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSole(SoleDto soleDTO)
        {
            if (soleDTO == null)
            {
                return BadRequest("Sole object is null");
            }

            var sole = new Sole
            {
                Name = soleDTO.Name,
                Description = soleDTO.Description
            };

            await _serviceManager.SoleService.AddSoleAsync(sole);
            return CreatedAtAction("GetSole", new { id = sole.Id }, sole);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSole(int id, SoleDto soleDTO)
        {
            if (soleDTO == null)
            {
                return BadRequest("Sole object is null");
            }

            var existingSole = await _serviceManager.SoleService.GetSoleByIdAsync(id);

            if (existingSole == null)
            {
                return NotFound("Sole not found");
            }

            existingSole.Name = soleDTO.Name;
            existingSole.Description = soleDTO.Description;

            await _serviceManager.SoleService.UpdateSoleAsync(existingSole);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSole(int id)
        {
            var sole = await _serviceManager.SoleService.GetSoleByIdAsync(id);

            if (sole == null)
            {
                return NotFound();
            }

            await _serviceManager.SoleService.RemoveSoleAsync(sole);
            return NoContent();
        }
    }
}
