using Domain.Entities;
using Domain.Repositories;
using EntitiesDto.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMaterialController : ControllerBase
    {
        private readonly IRepositoryManger _repositoryManger;

        public ProductMaterialController(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        [HttpGet("{materialId}/{productId}")]
        public async Task<IActionResult> GetProductMaterial(int materialId, string productId)
        {
            var productMaterial = await _repositoryManger.ProductMaterialRepository.GetAsync(materialId, productId);
            if (productMaterial == null)
            {
                return NotFound();
            }
            return Ok(productMaterial);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductMaterial(ProductMaterialCreateDTO productMaterialDTO)
        {
            if (productMaterialDTO == null)
            {
                return BadRequest("ProductMaterial object is null");
            }

            var productMaterial = new ProductMaterial
            {
                ProductId = productMaterialDTO.ProductId,
                MaterialId = productMaterialDTO.MaterialId
            };

            await _repositoryManger.ProductMaterialRepository.AddAsync(productMaterial);
            return CreatedAtAction("GetProductMaterial", new { materialId = productMaterial.MaterialId, productId = productMaterial.ProductId }, productMaterial);
        }


        [HttpPut("{materialId}")]
        public async Task<IActionResult> UpdateProductMaterial(int materialId, ProductMaterialCreateDTO productMaterialDTO)
        {
            if (productMaterialDTO == null)
            {
                return BadRequest("ProductMaterial object is null");
            }

            // Tìm MaterialId tương ứng với ProductId
            var materialIdForProduct = await _repositoryManger.ProductMaterialRepository.GetMaterialIdForProductAsync(productMaterialDTO.ProductId);

            if (materialIdForProduct == null)
            {
                return NotFound("Material not found for the given ProductId");
            }

            if (materialIdForProduct != materialId)
            {
                return BadRequest("MaterialId mismatch");
            }

            var updatedProductMaterial = new ProductMaterial
            {
                ProductId = productMaterialDTO.ProductId,
                MaterialId = materialId
            };

            await _repositoryManger.ProductMaterialRepository.UpdateAsync(updatedProductMaterial);
            return NoContent();
        }


        [HttpDelete("{materialId}/{productId}")]
        public async Task<IActionResult> DeleteProductMaterial(int materialId, string productId)
        {
            var productMaterial = await _repositoryManger.ProductMaterialRepository.GetAsync(materialId, productId);
            if (productMaterial == null)
            {
                return NotFound();
            }

            await _repositoryManger.ProductMaterialRepository.RemoveAsync(productMaterial);
            return NoContent();
        }
    }
}
