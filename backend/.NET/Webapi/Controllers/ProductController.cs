using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Linq;
using EntitiesDto.Product;
using Domain.Repositories;
using Persistence;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ProductController(IServiceManager serviceManager, IRepositoryManger repositoryManger, AppDbContext context)
        {
            _serviceManager = serviceManager;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct()
        {
            try
            {
                var products = await _serviceManager.ProductService.GetAllProductAsync();

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Product>> GetProduct(string Id)
        {
            var product = await _serviceManager.ProductService.GetByIdProduct(Id);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    RetailPrice = productDto.RetailPrice,
                    CostPrice = productDto.CostPrice,
                    Description = productDto.Description,
                    Brand = productDto.Brand,
                    DiscountRate = productDto.DiscountRate,
                    Status = productDto.Status,
                    SoleId = productDto.SoleId,
                    MaterialId = productDto.MaterialId,

                };

                var createdProduct = await _serviceManager.ProductService.CreateAsync(product);

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingProduct = await _serviceManager.ProductService.GetByIdProduct(id);

                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Name = productDto.Name;
                existingProduct.RetailPrice = productDto.RetailPrice;
                existingProduct.CostPrice = productDto.CostPrice;
                existingProduct.Description = productDto.Description;
                existingProduct.Brand = productDto.Brand;
                existingProduct.DiscountRate = productDto.DiscountRate;
                existingProduct.Status = productDto.Status;
                existingProduct.SoleId = productDto.SoleId;
                existingProduct.MaterialId = productDto.MaterialId;

                // ... Cập nhật thông tin khác của sản phẩm

                await _serviceManager.ProductService.UpdateByIdProduct(existingProduct.Id, existingProduct);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Tiếp tục trong ProductController

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterProducts(
     string sizeId, string colorId, string categoryId, int? materialId, int? soleId)
        {
            try
            {
                var filteredProducts = await _serviceManager.ProductService.FilterProductsAsync(
                    sizeId, colorId, categoryId, materialId, soleId);

                if (filteredProducts == null || !filteredProducts.Any())
                {
                    return NotFound();
                }

                return Ok(filteredProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

