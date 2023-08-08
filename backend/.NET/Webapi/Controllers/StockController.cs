﻿using System;
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


        // Các phương thức khác tại đây...
    }
}
    


