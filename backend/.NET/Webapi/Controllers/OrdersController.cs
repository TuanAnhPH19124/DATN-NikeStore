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
        [HttpGet("Confirm")]
        public async Task<ActionResult<IEnumerable<Order>>> GetConfirmedOrders()
        {
            try
            {
                var confirmedOrders = await _dbContext.Orders
                    .Where(o => o.OrderStatuses.Any(os => os.Status == StatusOrder.CONFIRM))
                    .ToListAsync();

                if (confirmedOrders == null || !confirmedOrders.Any())
                {
                    return NotFound();
                }

                return Ok(confirmedOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("PendingShip")]
        public async Task<ActionResult<IEnumerable<Order>>> GetPendingShipOrders()
        {
            try
            {
                var pendingShipOrders = await _dbContext.Orders
                    .Where(o => o.OrderStatuses.Any(os => os.Status == StatusOrder.PENDING_SHIP))
                    .ToListAsync();

                if (pendingShipOrders == null || !pendingShipOrders.Any())
                {
                    return NotFound();
                }

                return Ok(pendingShipOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Shipping")]
        public async Task<ActionResult<IEnumerable<Order>>> GetShippingOrders()
        {
            try
            {
                var shippingOrders = await _dbContext.Orders
                    .Where(o => o.OrderStatuses.Any(os => os.Status == StatusOrder.SHIPPING))
                    .ToListAsync();

                if (shippingOrders == null || !shippingOrders.Any())
                {
                    return NotFound();
                }

                return Ok(shippingOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Delivered")]
        public async Task<ActionResult<IEnumerable<Order>>> GetDeliveredOrders()
        {
            try
            {
                var deliveredOrders = await _dbContext.Orders
                    .Where(o => o.OrderStatuses.Any(os => os.Status == StatusOrder.DELIVERIED))
                    .ToListAsync();

                if (deliveredOrders == null || !deliveredOrders.Any())
                {
                    return NotFound();
                }

                return Ok(deliveredOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Decline")]
        public async Task<ActionResult<IEnumerable<Order>>> GetDeclinedOrders()
        {
            try
            {
                var declineOrders = await _dbContext.Orders
                    .Where(o => o.OrderStatuses.Any(os => os.Status == StatusOrder.DECLINE))
                    .ToListAsync();

                if (declineOrders == null || !declineOrders.Any())
                {
                    return NotFound();
                }

                return Ok(declineOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("pay")]
        public async Task<IActionResult> Payment([FromBody] OrderPostRequestDto orderDto)
        {
            if (ModelState.IsValid)
            {
                if (orderDto.PaymentMethod == ((int)PayMethod.Vnpay))
                {
                    #region Thanh toán vnpay

                    #endregion
                 
                }
                #region Đẩy dữ liệu vào db
                try
                {
                    await _service.OrderService.PostAndSendNontification(orderDto);
                    #region thông báo
                    await _hubContext.Clients.Group(ManagerHub.managerGroup).SendAsync("ReceiveMessage", "Khách hàng vừa đặt hàng cần xác nhận đơn hàng");
                    #endregion
                    return Ok();
                }
                catch (System.Exception)
                {
                    throw;
                }
                #endregion
            }
            return BadRequest();
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
                return Ok(new { Message = "Xác nhận đơn hàng thành công"});
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpGet("Get/{Id}")]
        public async Task<ActionResult<Order>> GetByIdOrder(string Id)
        {
            var order = await _service.OrderService.GetByIdOrderAsync(Id);

            if (order == null)
            {
                return NotFound();
            }
            return order;
        }
    }
}