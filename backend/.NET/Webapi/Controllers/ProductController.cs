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
        private readonly IRepositoryManger _repositoryManger;
        private readonly AppDbContext _context;


        public ProductController(IServiceManager serviceManager, IRepositoryManger repositoryManger, AppDbContext context)
        {
            _serviceManager = serviceManager;
            _repositoryManger = repositoryManger;
            _context = context;
        }
        //public Product CloneProduct(Product source)
        //{
        //    return new Product
        //    {
        //        Name = source.Name,
        //        RetailPrice = source.RetailPrice,
        //        Description = source.Description,
        //        Brand = source.Brand,
        //        DiscountRate = source.DiscountRate,
        //        Status = source.Status
        //        // Copy other properties here
        //    };
        //}



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProduct()
        {
            try
            {
                var products = await _serviceManager.ProductService.GetAllProductAsync();

                if (products == null || !products.Any())
                {
                    return NotFound();
                }

                // Chuyển đổi danh sách sản phẩm sang danh sách DTO
                var productDtos = products.Select(p => new ProductDto
                {

                    Name = p.Name,
                    DiscountRate = p.DiscountRate,
                    RetailPrice = p.RetailPrice,
                    Description = p.Description,
                    Brand = p.Brand,
                    Status = p.Status
                    // Thêm các trường khác tương ứng
                }).ToList();

                return Ok(productDtos);
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
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
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
                    Description = productDto.Description,
                    Brand = productDto.Brand,
                    DiscountRate = productDto.DiscountRate,
                    Status = productDto.Status
                };

                // ... Thêm các thông tin khác vào product

                var createdProduct = await _serviceManager.ProductService.CreateAsync(product);

                var createdProductDto = new ProductDto
                {
                    Name = createdProduct.Name,
                    RetailPrice = createdProduct.RetailPrice,
                    Description = createdProduct.Description,
                    Brand = createdProduct.Brand,
                    DiscountRate = createdProduct.DiscountRate,
                    Status = createdProduct.Status
                    // Không cần thêm thông tin liên quan đến Stock và Category
                };

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProductDto);
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
                existingProduct.Description = productDto.Description;
                existingProduct.Brand = productDto.Brand;
                existingProduct.DiscountRate = productDto.DiscountRate;
                existingProduct.Status = productDto.Status;

                // ... Cập nhật thông tin khác của sản phẩm

                await _serviceManager.ProductService.UpdateByIdProduct(existingProduct.Id, existingProduct);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }







    }
}

