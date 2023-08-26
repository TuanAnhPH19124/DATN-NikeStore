using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Product;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Persistence;
using Persistence.Ultilities;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            _serviceManager=serviceManager;
            _dbContext=dbContext;
        }
   

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDtoForGet>>> GetAllProduct()
        {
            try
            {
                var products = await _serviceManager.ProductService.GetAllProductAsync();
                var productDtos = products.Select(product => new ProductDtoForGet
                {
                    Name = product.Name,
                    RetailPrice = product.RetailPrice,
                    Description = product.Description,
                    DiscountRate = product.DiscountRate,
                    SoleId = product.SoleId,
                    MaterialId = product.MaterialId,
                    Colors = product.ProductImages.GroupBy(pi => pi.ColorId).Select(group => new ColorDtoForGet
                    {
                        Id = group.Key,
                        Images = group.Select(pi => new ImageDtoForGet
                        {
                            ImageUrl = pi.ImageUrl,
                            SetAsDefault = pi.SetAsDefault
                        }).ToList(),
                        Sizes = product.Stocks.Where(stock => stock.ColorId == group.Key).Select(stock => new SizeDtoForGet
                        {
                            Id = stock.SizeId,
                            UnitInStock = stock.UnitInStock
                        }).ToList()
                    }).ToList(),
                    Categories = product.CategoryProducts.Select(category => new CategoryDtoForGet
                    {
                        Id = category.CategoryId
                    }).ToList()
                }).ToList();

                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message
                });
            }
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<Product>> GetProduct(string Id)
        {
            try
            {
                var product = await _serviceManager.ProductService.GetByIdProduct(Id);
                if (product == null)
                {
                    return NotFound();
                }

                var productDto = new ProductDtoForGet
                {
                    Name = product.Name,
                    RetailPrice = product.RetailPrice,
                    Description = product.Description,
                    DiscountRate = product.DiscountRate,
                    SoleId = product.SoleId,
                    MaterialId = product.MaterialId,
                    Colors = product.ProductImages.GroupBy(pi => pi.ColorId).Select(group => new ColorDtoForGet
                    {
                        Id = group.Key,
                        Images = group.Select(pi => new ImageDtoForGet
                        {
                            ImageUrl = pi.ImageUrl,
                            SetAsDefault = pi.SetAsDefault
                        }).ToList(),
                        Sizes = product.Stocks.Where(stock => stock.ColorId == group.Key).Select(stock => new SizeDtoForGet
                        {
                            Id = stock.SizeId,
                            UnitInStock = stock.UnitInStock
                        }).ToList()
                    }).ToList(),
                    Categories = product.CategoryProducts.Select(category => new CategoryDtoForGet
                    {
                        Id = category.CategoryId
                    }).ToList()
                };

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message
                });
            }
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
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductAPI productDto)
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
        
                existingProduct.DiscountRate = productDto.DiscountRate;
     
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

