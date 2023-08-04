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
                    DiscountRate=p.DiscountRate,
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
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductForPostProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Tạo đối tượng Product từ DTO
                var product = new Product
                {
                    Name = productDto.Name,
                    RetailPrice = productDto.RetailPrice,
                    Description = productDto.Description,
                    Brand = productDto.Brand,
                    DiscountRate = productDto.DiscountRate,
                    Status = productDto.Status
                };

                foreach (var stockDto in productDto.Stocks)
                {
                    var colorId = await _serviceManager.ColorService.GetByIdColorAsync(stockDto.ColorId);
                    var sizeId = await _serviceManager.SizeService.GetByIdSizeAsync(stockDto.SizeId);

                    // Thêm thông tin số lượng, size và màu sắc vào stock
                    product.Stocks.Add(new Stock
                    {
                        ColorId = stockDto.ColorId,
                        SizeId = stockDto.SizeId,
                        UnitInStock = stockDto.UnitInStock
                    });
                }

                var createdProduct = await _serviceManager.ProductService.CreateAsync(product);

                // Tạo đối tượng ProductDto để trả về
                var createdProductDto = new ProductDto
                {

                    Name = createdProduct.Name,
                    RetailPrice = createdProduct.RetailPrice,
                    Description = createdProduct.Description,
                    Brand = createdProduct.Brand,
                    DiscountRate = createdProduct.DiscountRate,
                    Status = createdProduct.Status,
                    // Không cần thêm liên kết đối tượng Stocks và CategoryIds
                };

                // Trả về kết quả thêm mới sản phẩm và đối tượng DTO đã tạo
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProductDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }








        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductForUpdateProductDto productDto)
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

                // Cập nhật thông tin sản phẩm từ DTO
                existingProduct.Name = productDto.Name;
                existingProduct.RetailPrice = productDto.RetailPrice;
                existingProduct.Description = productDto.Description;
                existingProduct.Brand = productDto.Brand;
                existingProduct.DiscountRate = productDto.DiscountRate;
                existingProduct.Status = productDto.Status;

                // Xóa các Stock cũ và thêm lại các Stock mới
                existingProduct.Stocks.Clear();
                foreach (var stockDto in productDto.Stocks)
                {
                    var colorId = await _serviceManager.ColorService.GetByIdColorAsync(stockDto.ColorId);
                    var sizeId = await _serviceManager.SizeService.GetByIdSizeAsync(stockDto.SizeId);

                    // Thêm thông tin số lượng, size và màu sắc vào stock
                    existingProduct.Stocks.Add(new Stock
                    {
                        ColorId = stockDto.ColorId,
                        SizeId = stockDto.SizeId,
                        UnitInStock = stockDto.UnitInStock
                    });
                }

                // Cập nhật thông tin sản phẩm trong cơ sở dữ liệu
                await _serviceManager.ProductService.UpdateByIdProduct(existingProduct.Id,existingProduct);

                return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}

