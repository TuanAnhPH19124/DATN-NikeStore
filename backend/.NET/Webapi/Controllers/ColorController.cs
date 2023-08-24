using EntitiesDto;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using Domain.Entities;
using System.Linq;
using Mapster;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public ColorController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Color>>> GetAllColor()
        {
            try
            {
                var color = await _serviceManager.ColorService.GetAllColorAsync();
                if (color == null || !color.Any())
                {
                    return NotFound();
                }
                return Ok(color);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("Get/{Id}")]
        public async Task<ActionResult<Color>> GetByIdColor(string Id)
        {
            var color = await _serviceManager.ColorService.GetByIdColorAsync(Id);

            if (color == null)
            {
                return NotFound();
            }
            return color;
        }


        [HttpPost]
        public async Task<IActionResult> CreateColor(ColorCreateDto colorCreateDto)
        {
            try
            {
                
                var color = colorCreateDto.Adapt<Color>();
                await _serviceManager.ColorService.CreateAsync(color);
                return CreatedAtAction(nameof(GetByIdColor), new { id = color.Id }, color);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColor(string id, ColorDto colorDto)
        {
            if (id != colorDto.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                var color = colorDto.Adapt<Color>();
                await _serviceManager.ColorService.UpdateByIdColor(id, color);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
