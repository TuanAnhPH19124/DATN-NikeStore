using Domain.Entities;
using Domain.Enums;
using EntitiesDto.Order;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Service;
using Service.Abstractions;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webapi.Hubs;
using Domain.DTOs;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Webapi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IServiceManager _service;
        private readonly IHubContext<ManagerHub> _hubContext;
        private readonly AppDbContext _dbContext;

        public OrdersController(ILogger<OrdersController> logger, IServiceManager service, IHubContext<ManagerHub> hubContext, AppDbContext dbContext)
        {
            _logger=logger;
            _service=service;
            _hubContext=hubContext;
            _dbContext=dbContext;
        }

        [HttpPost("PayAtStore")]
        public async Task<IActionResult> PayAtStore([FromBody] OrderAtStorePostRequestDto orderPost)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return await Task.FromResult(BadRequest(new { Errors = errors }));
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _service.OrderService.PostNewOrderAtStore(orderPost);
                    transaction.Commit();
                    return Ok();
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                    throw;
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrder()
        {
            try
            {
                var order = await _service.OrderService.GetAllOrderAsync();
                if (order == null || !order.Any())
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetConfirmedOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetConfirmedOrders()
        {
            try
            {
                var confirmedOrders = await _service.OrderService.GetAllOrderAsync();
                if (confirmedOrders == null || !confirmedOrders.Any())
                {
                    return NotFound();
                }

                var confirmedOrdersByStatus = confirmedOrders
                    .Where(order => order.OrderStatuses.Any(c=>c.Status == StatusOrder.CONFIRM))
                    .ToList();

                if (!confirmedOrdersByStatus.Any())
                {
                    return NotFound("No confirmed orders found.");
                }

                return Ok(confirmedOrdersByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetPendingShipOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPendingShipOrders()
        {
            try
            {
                var pendingShipOrders = await _service.OrderService.GetAllOrderAsync();
                if (pendingShipOrders == null || !pendingShipOrders.Any())
                {
                    return NotFound();
                }

                var pendingShipOrdersByStatus = pendingShipOrders
                   .Where(order => order.OrderStatuses.Any(c => c.Status == StatusOrder.PENDING_SHIP))
                    .ToList();

                if (!pendingShipOrdersByStatus.Any())
                {
                    return NotFound("No pending ship orders found.");
                }

                return Ok(pendingShipOrdersByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetShippingOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetShippingOrders()
        {
            try
            {
                var shippingOrders = await _service.OrderService.GetAllOrderAsync();
                if (shippingOrders == null || !shippingOrders.Any())
                {
                    return NotFound();
                }

                var shippingOrdersByStatus = shippingOrders
                   .Where(order => order.OrderStatuses.Any(c => c.Status == StatusOrder.SHIPPING))
                    .ToList();

                if (!shippingOrdersByStatus.Any())
                {
                    return NotFound("No shipping orders found.");
                }

                return Ok(shippingOrdersByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetDeliveredOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetDeliveredOrders()
        {
            try
            {
                var deliveredOrders = await _service.OrderService.GetAllOrderAsync();
                if (deliveredOrders == null || !deliveredOrders.Any())
                {
                    return NotFound();
                }

                var deliveredOrdersByStatus = deliveredOrders
                    .Where(order => order.OrderStatuses.Any(c => c.Status == StatusOrder.DELIVERIED))
                    .ToList();

                if (!deliveredOrdersByStatus.Any())
                {
                    return NotFound("No delivered orders found.");
                }

                return Ok(deliveredOrdersByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetDeclinedOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetDeclinedOrders()
        {
            try
            {
                var declinedOrders = await _service.OrderService.GetAllOrderAsync();
                if (declinedOrders == null || !declinedOrders.Any())
                {
                    return NotFound();
                }

                var declinedOrdersByStatus = declinedOrders
                  .Where(order => order.OrderStatuses.Any(c => c.Status == StatusOrder.DECLINE))
                    .ToList();

                if (!declinedOrdersByStatus.Any())
                {
                    return NotFound("No declined orders found.");
                }

                return Ok(declinedOrdersByStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("payOneline")]
        public async Task<IActionResult> Payment([FromBody] OrderPostRequestDto orderDto)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _service.OrderService.CreateNewOnlineOrder(orderDto);
                    #region thông báo
                    await _hubContext.Clients.Group(ManagerHub.managerGroup).SendAsync("ReceiveMessage", "Khách hàng vừa đặt hàng cần xác nhận đơn hàng");
                    #endregion
                    transaction.Commit();
                    return Ok();    
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPut("confirmOrder/{id}")]
        public async Task<IActionResult> ConfirmNewOrder(string id, [FromBody]OrderConfirmPutRequestDto OrderDto){
            if (!ModelState.IsValid){
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return await Task.FromResult(BadRequest(new {Errors = errors}));
            }
            try
            {
                await _service.OrderService.UpdateOrderOnConfirm(id, OrderDto);
                #region Gửi Thông báo đến người đặt
                    
                #endregion
                return Ok(new { Message = "Đã cập nhật trạng thái đơn hàng"});
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpGet("Get/{Id}")]
        public async Task<ActionResult> GetByIdOrder(string Id)
        {
            var order = await _service.OrderService.GetByIdOrderAsync(Id);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
    }
}