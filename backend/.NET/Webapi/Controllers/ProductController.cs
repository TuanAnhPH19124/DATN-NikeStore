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

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProductController(IServiceManager serviceManager)
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
                return Ok(products); // Thêm danh sách sản phẩm vào đây
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
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductForPostProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = new Product
                {
                    BarCode = productDto.BarCode,
                    Name = productDto.Name,
                    RetailPrice = productDto.RetailPrice,
                    Description = productDto.Description,
                    Brand = productDto.Brand,
                    DiscountRate = productDto.DiscountRate
                };

                var createdProduct = await _serviceManager.ProductService.CreateAsync(product);

                // Trả về kết quả thêm mới sản phẩm và đường dẫn đến sản phẩm đã tạo
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("The provided id does not match the id in the user data.");
            }
            try
            {
                // Gọi phương thức cập nhật sản phẩm từ dịch vụ
                var updatedProduct = await _serviceManager.ProductService.UpdateByIdProduct(id, product);

                // Kiểm tra xem sản phẩm đã tồn tại và cập nhật thành công hay chưa
                if (updatedProduct == null)
                {
                    return NotFound(); // Sản phẩm không tồn tại trong cơ sở dữ liệu
                }

                return NoContent(); // Cập nhật thành công
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ DbUpdateConcurrencyException tại đây
                return StatusCode((int)HttpStatusCode.Conflict, ex);
            }
        }
    }
}
