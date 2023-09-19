using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using EntitiesDto.Stock;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public StockController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("GetStockByRelationId/{productId}/{colorId}/{sizeId}")]
        public async Task<ActionResult> GetStockByRelationId(string productId, string colorId, string sizeId)
        {
            if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(colorId) || string.IsNullOrEmpty(sizeId))
                return BadRequest(new {error = "Không được bỏ trống các khóa ngoại"});

            var stock = await _serviceManager.StockService.GetStockByRelation(productId, colorId, sizeId);
            return Ok(stock);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetAllStocks()
        {
            try
            {
                var stocks = await _serviceManager.StockService.GetAllStocksAsync();

                if (stocks == null || !stocks.Any())
                {
                    return NotFound();
                }

                return Ok(stocks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Stock>> GetStock(string productId)
        {
            var stock = await _serviceManager.StockService.GetStockByIdAsync(productId);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }

            [HttpPost]
            public async Task<ActionResult> CreateStock([FromBody] StockDto stockDto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var stock = new Stock
                    {
                        ProductId = stockDto.ProductId,
                        ColorId = stockDto.ColorId,
                        SizeId = stockDto.SizeId,
                        UnitInStock = stockDto.UnitInStock
                    };

                    await _serviceManager.StockService.AddStockAsync(stock);

                    return CreatedAtAction(nameof(GetStock), new { productId = stockDto.ProductId, colorId = stockDto.ColorId, sizeId = stockDto.SizeId }, stockDto);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

        [HttpPost("getStockId")]
        public async Task<ActionResult> GetStockId([FromBody]GetStockIdAPI item){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.StockService.GetStockIdList(item);

            return Ok(result);
        }        

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteStockByProductId(string productId)
        {
            try
            {
                await _serviceManager.StockService.DeleteStockAsync(productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
    


