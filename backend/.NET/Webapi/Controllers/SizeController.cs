using Domain.Entities;
using EntitiesDto.User;
using EntitiesDto;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;
using EntitiesDto.Product;
using Mapster;


namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public SizeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Size>>> GetAllSize()
        {
            try
            {
                var size = await _serviceManager.SizeService.GetAllSizeAsync();
                if (size == null || !size.Any())
                {
                    return NotFound();
                }
                return Ok(size);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet("Get/{Id}")]
        public async Task<ActionResult<Size>> GetByIdSize(string Id)
        {
            var size = await _serviceManager.SizeService.GetByIdSizeAsync(Id);

            if (size == null)
            {
                return NotFound();
            }
            return size;
        }

        [HttpGet("GetSizeForProduct/{ProductId}/{colorId}")]
        public async Task<ActionResult> GetSizeForProduct(string ProductId, string colorId){
            if (string.IsNullOrEmpty(ProductId))
                return BadRequest(new {error = "Không được để Id là rỗng hoặc chuỗi rỗng"});
            
            if (string.IsNullOrEmpty(colorId))
                return BadRequest(new {error = "Không được để colorId là rỗng hoặc chuỗi rỗng"});

            var result = await _serviceManager.SizeService.GetSizeForProduct(ProductId, colorId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize(SizeDto sizeDto)
        {
            if (sizeDto == null)
            {
                return BadRequest("Sole object is null");
            }

            var existingSize = await _serviceManager.SizeService.GetByNumberSizeAsync(sizeDto.NumberSize);
            if (existingSize != null)
            {
                return Conflict("Size with the same NumberSize already exists");
            }

            var size = new Size
            {
                NumberSize = sizeDto.NumberSize,
                Description = sizeDto.Description
            };

            await _serviceManager.SizeService.CreateAsync(size);
            return CreatedAtAction("GetByIdSize", new { Id = size.Id}, size);
        }      

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSize(string id, SizeDtoUpdate sizeDtoUpdate)
        {
            if (id != sizeDtoUpdate.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                var size = sizeDtoUpdate.Adapt<Size>();
                await _serviceManager.SizeService.UpdateByIdSize(id, size);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
            return NoContent();
        }
    }
}
