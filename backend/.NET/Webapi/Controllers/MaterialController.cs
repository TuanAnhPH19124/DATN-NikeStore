using Domain.Entities;
using EntitiesDto.Material;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public MaterialController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterial(int id)
        {
            var material = await _serviceManager.MaterialService.GetMaterialByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMaterials()
        {
            var materials = await _serviceManager.MaterialService.GetAllMaterialsAsync();
            return Ok(materials);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, MaterialCreateDTO materialDTO)
        {
            if (materialDTO == null)
            {
                return BadRequest("Material object is null");
            }

            var existingMaterial = await _serviceManager.MaterialService.GetMaterialByIdAsync(id);

            if (existingMaterial == null)
            {
                return NotFound("Material not found");
            }

            // Cập nhật thông tin vật liệu từ DTO
            existingMaterial.Name = materialDTO.Name;
            existingMaterial.Description = materialDTO.Description;
            // Cập nhật các thuộc tính khác tùy theo yêu cầu của Material

            await _serviceManager.MaterialService.UpdateMaterialAsync(existingMaterial);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial(MaterialCreateDTO materialDTO)
        {
            if (materialDTO == null)
            {
                return BadRequest("Material object is null");
            }

            var material = new Material
            {
                Name = materialDTO.Name,
                Description = materialDTO.Description
                // Thêm các thuộc tính khác tùy theo yêu cầu của Material
            };

            await _serviceManager.MaterialService.AddMaterialAsync(material);
            return CreatedAtAction("GetMaterial", new { id = material.Id }, material);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _serviceManager.MaterialService.GetMaterialByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            await _serviceManager.MaterialService.RemoveMaterialAsync(material);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMaterials([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is empty");
            }

            Expression<Func<Material, bool>> predicate = m =>
                m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm);

            var materials = await _serviceManager.MaterialService.FindMaterialsAsync(predicate);
            return Ok(materials);
        }
    }
}
