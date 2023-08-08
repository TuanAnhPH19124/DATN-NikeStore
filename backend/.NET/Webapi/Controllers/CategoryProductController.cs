using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using EntitiesDto.CategoryDto;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoryProductController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryProduct>>> GetAllCategoryProducts()
        {
            try
            {
                var categoryProducts = await _serviceManager.CategoryProductService.GetAllCategoryProductsAsync();

                if (categoryProducts == null || !categoryProducts.Any())
                {
                    return NotFound();
                }

                return Ok(categoryProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult<CategoryProductDto>> GetCategoryProduct(string productId)
        {
            try
            {
                // Lấy danh sách CategoryProduct có ProductId tương ứng
                var categoryProducts = await _serviceManager.CategoryProductService.GetAllCategoryProductsAsync();
                var categoryProductsOfProduct = categoryProducts.Where(cp => cp.ProductId == productId);

                // Chuyển đổi danh sách CategoryProduct sang danh sách CategoryProductDto
                var categoryProductDtos = categoryProductsOfProduct.Select(cp => new CategoryProductDto
                {
                    ProductId = cp.ProductId,
                    CategoryId = cp.CategoryId,
                });

                return Ok(categoryProductDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
            public async Task<ActionResult> CreateCategoryProduct([FromBody] CategoryProductDto categoryProductDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var categoryProduct = new CategoryProduct
                    {
                        ProductId = categoryProductDto.ProductId,
                        CategoryId = categoryProductDto.CategoryId
                    };

                    await _serviceManager.CategoryProductService.CreateCategoryProductAsync(categoryProduct);

                    return CreatedAtAction(nameof(GetCategoryProduct), new { productId = categoryProductDto.ProductId, categoryId = categoryProductDto.CategoryId }, categoryProductDto);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpPut("{productId}/{categoryId}")]
            public async Task<ActionResult> UpdateCategoryProduct(string productId, string categoryId, [FromBody] CategoryProductDto categoryProductDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var existingCategoryProduct = await _serviceManager.CategoryProductService.GetCategoryProductByIdAsync(productId, categoryId);

                    if (existingCategoryProduct == null)
                    {
                        return NotFound();
                    }

                    // Update the properties as needed
                    existingCategoryProduct.ProductId = categoryProductDto.ProductId;
                    existingCategoryProduct.CategoryId = categoryProductDto.CategoryId;

                    await _serviceManager.CategoryProductService.UpdateCategoryProductAsync(productId, categoryId, existingCategoryProduct);

                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

        // Các phương thức khác tại đây...
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteCategoryProduct(string productId)
        {
            try
            {
                // Lấy danh sách CategoryProduct có ProductId tương ứng và xóa chúng
                var categoryProducts = await _serviceManager.CategoryProductService.GetAllCategoryProductsAsync();
                var categoryProductsToDelete = categoryProducts.Where(cp => cp.ProductId == productId);

                foreach (var categoryProduct in categoryProductsToDelete)
                {
                    await _serviceManager.CategoryProductService.DeleteCategoryProductAsync(categoryProduct.ProductId, categoryProduct.CategoryId);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
    




