using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using EntitiesDto.Images;
using EntitiesDto.Product;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Persistence;
using Persistence.Ultilities;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly AppDbContext _dbContext;

        public ProductController(IServiceManager serviceManager, AppDbContext dbContext)
        {
            _serviceManager = serviceManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsForDisplayAsync()
        {
            var productsForDisplay = await _serviceManager.ProductService.GetAllProductAsync();

            return Ok(productsForDisplay);
        }

        [HttpGet("1Image")]
        public async Task<IActionResult> GetAllProductsForDisplayImageAsync()
        { 
            var product = await _serviceManager.ProductService.GetAllProductImageAsync();
            return Ok(product);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync(string productId)
        {
            var productDto = await _serviceManager.ProductService.GetProductByIdAsync(productId);
            return Ok(productDto);
        }
            return Ok(productDto);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveProductsAsync()
        {
            var products = await _serviceManager.ProductService.GetAllProductAsync();

            var activeProducts = products
                .Where(product => product.Status == Status.ACTIVE) // Lọc ra các sản phẩm có trạng thái là Active
                .ToList();

            return Ok(activeProducts);
        }




        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductAPI productAPI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = string.Empty;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var nProduct = productAPI.Adapt<Product>();
                    productId = nProduct.Id;
                    nProduct.ProductImages = new List<ProductImage>();
                    nProduct.Stocks = new List<Stock>();
                    nProduct.CategoryProducts = new List<CategoryProduct>();
                    var UrList = UploadService.UploadImages(productAPI.Colors, nProduct.Id);

                    foreach (var urlParent in UrList)
                    {
                        foreach (var urlChild in urlParent.Value)
                        {
                            nProduct.ProductImages.Add(new ProductImage
                            {
                                ColorId = urlParent.Key,
                                ImageUrl = urlChild.Key,
                                SetAsDefault = urlChild.Value,
                                ProductId = nProduct.Id
                            });
                        }
                    }

                    nProduct.Stocks = (
                        from color in productAPI.Colors
                        from size in color.Sizes
                        select new Stock
                        {
                            UnitInStock = size.UnitInStock,
                            ColorId = color.Id,
                            SizeId = size.Id
                        }
                        ).ToList();

                    nProduct.CategoryProducts = productAPI.Categories.Select(item => new CategoryProduct
                    {
                        CategoryId = item.Id,
                    }).ToList();

                    var createdProduct = await _serviceManager.ProductService.CreateAsync(nProduct);
                    transaction.Commit();


                    //return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
                    return Ok();
                }
                catch (Exception ex)
                {
                    UploadService.RollBack(productId);
                    transaction.Rollback();
                    return BadRequest(new
                    {
                        Error = ex.Message
                    });
                    throw;
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromForm] ProductUpdateAPI productAPI)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != productAPI.Id)
                return BadRequest(new
                {
                    error = "Id sản phẩm không khớp!"
                });
            var targetProduct = productAPI.Adapt<Product>();
            targetProduct.CategoryProducts = new List<CategoryProduct>();
            targetProduct.ProductImages = new List<ProductImage>();
            targetProduct.Stocks = new List<Stock>();

            targetProduct.CategoryProducts = productAPI.Categories.Select(p => new CategoryProduct
            {
                CategoryId = p.Id,
                ProductId = id
            }).ToList();

            targetProduct.Stocks = (
            from color in productAPI.Colors
            from size in color.Sizes
            select new Stock
            {
                UnitInStock = size.UnitInStock,
                ColorId = color.Id,
                SizeId = size.Id
            }
            ).ToList();

            var tempId = Guid.NewGuid().ToString();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var UrList = UploadService.UploadImages(productAPI.Colors, id, tempId);

                    foreach (var urlParent in UrList)
                    {
                        foreach (var urlChild in urlParent.Value)
                        {
                            targetProduct.ProductImages.Add(new ProductImage
                            {
                                ColorId = urlParent.Key,
                                ImageUrl = urlChild.Key,
                                SetAsDefault = urlChild.Value,
                                ProductId = id
                            });
                        }
                    }

                    await _serviceManager.ProductService.UpdateByIdProduct(id, targetProduct);
                    transaction.Commit();
                    UploadService.Rename(tempId, id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    UploadService.RollBack(tempId);
                    return BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            }
        }

        [HttpGet("filter")]
        public async Task<ActionResult> FilterProducts([FromQuery] ProductFilterOptionAPI options)
        {
            var products = await _serviceManager.ProductService.FilterProductsAsync(options);
            return Ok(products);
        }
    }
}




