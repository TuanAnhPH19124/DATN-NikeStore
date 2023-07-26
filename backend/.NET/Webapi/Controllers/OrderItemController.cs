using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using EntitiesDto;
using Microsoft.AspNetCore.Http;
using Service.Abstractions;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
        public class OrderItemController : ControllerBase
        {
            private readonly IServiceManager _serviceManager;
            public OrderItemController(IServiceManager serviceManager)
            {
                _serviceManager = serviceManager;
            }
            // so luong sp da ban
            [HttpGet("product/{productId}/amount")]
            public async Task<IActionResult> GetSoldAmount(string productId)
            {
                int amount = await _serviceManager.OrderItemService.GetAmountByProductId(productId);
                return Ok(amount);
            }
            // doanh thu cua 1 sp
            [HttpGet("product/{productId}/revenue")]
            public async Task<IActionResult> GetRevenue(string productId)
            {
                decimal revenue = await _serviceManager.OrderItemService.GetRevenueByProductId(productId);

                return Ok(revenue);
            }
            // doanh thu tong 
            [HttpGet("total-revenue")]
            public async Task<IActionResult> GetTotalRevenueInTimeRange(DateTime startDate, DateTime endDate)
            {
            decimal totalRevenue = await _serviceManager.OrderItemService.GetTotalRevenueInTimeRange(startDate, endDate);
            return Ok(totalRevenue);
            }
        }
}
