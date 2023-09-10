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
using Service;
using Domain.Repositories;
using Nest;

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
            // tong so luong sp da ban cua 1 sp cu the
        [HttpGet("product/{productId}/amount")]
        public async Task<IActionResult> GetSoldAmount(string productId)
        {
            int amount = await _serviceManager.OrderItemService.GetAmountByProductId(productId);
            return Ok(amount);
        }
            // doanh thu cua 1 sp cu the
        [HttpGet("product/{productId}/revenue")]
        public async Task<IActionResult> GetRevenue(string productId)
        {
            decimal revenue = await _serviceManager.OrderItemService.GetRevenueByProductId(productId);

            return Ok(revenue);
        }
            // doanh thu tong trong 1 khoang thoi gian
        [HttpGet("total-time")]
        public async Task<IActionResult> GetTotalOrder(DateTime startDate, DateTime endDate)
        {
             float totalOrder = await _serviceManager.OrderItemService.GetTotalOrder(startDate, endDate);
             return Ok(totalOrder);
        }
            // doanh so ban hang cua thang hien tai
        [HttpGet("total-month")]
        public async Task<IActionResult> GetTotalOrdersForCurrentMonth()
        {
              float totalOrderMonth = await _serviceManager.OrderItemService.GetTotalOrdersForCurrentMonth();
              return Ok(totalOrderMonth);
        }

        // doanh so ban hang cua ngay hom nay
        [HttpGet("total-date")]
        public async Task<IActionResult> GetTotalOrdersForCurrentDate()
        {
            float totalOrderDate = await _serviceManager.OrderItemService.GetTotalOrdersForCurrentDate();
            return Ok(totalOrderDate);
        }

        // Tong so don hang cua ngay hom nay
        [HttpGet("total-bill-date")]
        public async Task<IActionResult> GetTotalBillForCurrentDate()
        {
            float totalBillDate = await _serviceManager.OrderItemService.GetTotalBillForCurrentDate();
            return Ok(totalBillDate);
        }

        // Tong so don hang cua thang hien tai
        [HttpGet("total-bill-month")]
        public async Task<IActionResult> GetTotalBillForCurrentMonth()
        {
            float totalBillMonth = await _serviceManager.OrderItemService.GetTotalBillForCurrentMonth();
            return Ok(totalBillMonth);
        }

        // Tong so hoa don trong 1 khoang thoi gian
        [HttpGet("total-bill-time")]
        public async Task<IActionResult> GetTotalOrdersInTimeRange(DateTime startDate, DateTime endDate)
        {
            float totalBillAll = await _serviceManager.OrderItemService.GetTotalOrdersInTimeRange(startDate, endDate);
            return Ok(totalBillAll);
        }

        // Tong so hoa don da ban
        [HttpGet("total-bill-all")]
        public async Task<IActionResult> GetTotalBill()
        {
            float totalBillAll = await _serviceManager.OrderItemService.GetTotalBill();
            return Ok(totalBillAll);
        }

        // Tong so doanh thu
        [HttpGet("total-all")]
        public async Task<IActionResult> GetTotalAmount()
        {
            float totalAll = await _serviceManager.OrderItemService.GetTotalAmount();
            return Ok(totalAll);
        }

        // select 5 san pham ban duoc nhieu nhat
        [HttpGet("get-top")]
        public async Task<ActionResult<List<ProductSalesAndRevenueInfo>>> GetTopSellingProductsAndRevenue()
        {
            try
            {
                var topProducts = await _serviceManager.OrderItemService.GetTopSellingProductsAndRevenue(5);
                return Ok(topProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
