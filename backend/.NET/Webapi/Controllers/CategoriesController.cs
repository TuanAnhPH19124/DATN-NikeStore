using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.CategoryDto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepositoryManger _repositoryManger;

        public CategoriesController(IRepositoryManger repositoryManger)
        {
            _repositoryManger=repositoryManger;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var categories = await _repositoryManger.CategoryRepository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return await Task.FromResult(BadRequest("Id danh mục không được null hoặc trống."));
            }
            var targetC = await _repositoryManger.CategoryRepository.GetByIdAsync(Id);
            if (targetC == null)
            {
                return await Task.FromResult(NotFound());
            }
            return Ok(targetC);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CategoryPostDto category)
        {
            if (!ModelState.IsValid)
            {
                return await Task.FromResult(BadRequest());
            }
            var mapC = category.Adapt<Category>();
            await _repositoryManger.CategoryRepository.AddAsync(mapC);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]CategoryPostDto category)
        {
            if (!ModelState.IsValid)
            {
                return await Task.FromResult(BadRequest());
            }
            var mapC = category.Adapt<Category>();
            await _repositoryManger.CategoryRepository.UpdateAsync(id, mapC);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                await _repositoryManger.CategoryRepository.DeleteAsync(id);
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
