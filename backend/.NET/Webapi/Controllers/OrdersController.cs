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
                var confirmedOrderStatuses = await _service.OrderService.GetAllOrderAsync();

                if (confirmedOrderStatuses == null || !confirmedOrderStatuses.Any())
                {
                    return NotFound();
                }

                var confirmedOrdersWithLatestStatus = new List<OrderDto>();

                foreach (var order in confirmedOrderStatuses)
                {
                    // Lấy trạng thái mới nhất có trạng thái CONFIRM cho mỗi đơn hàng
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Time)
                        .FirstOrDefault(status => status.Status == StatusOrder.CONFIRM);

                    if (latestStatus != null)
                    {
                        var confirmedOrder = new OrderDto
                        {
                            // Copy thông tin từ order vào confirmedOrder
                            Id = order.Id,
                            Address = order.Address,
                            PhoneNumber = order.PhoneNumber,
                            Note = order.Note,
                            Paymethod = order.Paymethod,
                            Amount = order.Amount,
                            CustomerName = order.CustomerName,
                            DateCreated = order.DateCreated,
                            PassivedDate = order.PassivedDate,
                            ModifiedDate = order.ModifiedDate,
                            UserId = order.UserId,
                            EmployeeId = order.EmployeeId,
                            VoucherId = order.VoucherId,
                            OrderStatuses = new List<OrderStatusDto> { latestStatus },
                            OrderItems = order.OrderItems.Select(item => new OrderItemDto
                            {
                                OrderId = item.OrderId,
                                ProductId = item.ProductId,
                                ColorId = item.ColorId,
                                SizeId = item.SizeId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            }).ToList()
                        };

                        confirmedOrdersWithLatestStatus.Add(confirmedOrder);
                    }
                }

                if (!confirmedOrdersWithLatestStatus.Any())
                {
                    return NotFound("No confirmed orders found.");
                }

                return Ok(confirmedOrdersWithLatestStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }








        [HttpGet("GetLatestPendingShipOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestPendingShipOrders()
        {
            try
            {
                var pendingShipOrders = await _service.OrderService.GetAllOrderAsync();

                if (pendingShipOrders == null || !pendingShipOrders.Any())
                {
                    return NotFound();
                }

                var latestPendingShipOrders = new List<OrderDto>();

                foreach (var order in pendingShipOrders)
                {
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Time)
                        .FirstOrDefault(status => status.Status == StatusOrder.PENDING_SHIP);

                    if (latestStatus != null)
                    {
                        var latestPendingShipOrder = new OrderDto
                        {
                            Id = order.Id,                          
                            Address = order.Address,
                            PhoneNumber = order.PhoneNumber,
                            Note = order.Note,
                            Paymethod = order.Paymethod,
                            Amount = order.Amount,
                            CustomerName = order.CustomerName,
                            DateCreated = order.DateCreated,
                            PassivedDate = order.PassivedDate,
                            ModifiedDate = order.ModifiedDate,
                            UserId = order.UserId,
                            EmployeeId = order.EmployeeId,
                            VoucherId = order.VoucherId,
                            OrderStatuses = new List<OrderStatusDto> { latestStatus },
                            OrderItems = order.OrderItems.Select(item => new OrderItemDto
                            {
                                OrderId = item.OrderId,
                                ProductId = item.ProductId,
                                ColorId = item.ColorId,
                                SizeId = item.SizeId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            }).ToList()
                        };

                        latestPendingShipOrders.Add(latestPendingShipOrder);
                    }
                }

                if (!latestPendingShipOrders.Any())
                {
                    return NotFound("No latest pending ship orders found.");
                }

                return Ok(latestPendingShipOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetLatestShippingOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestShippingOrders()
        {
            try
            {
                var shippingOrders = await _service.OrderService.GetAllOrderAsync();

                if (shippingOrders == null || !shippingOrders.Any())
                {
                    return NotFound();
                }

                var latestShippingOrders = new List<OrderDto>();

                foreach (var order in shippingOrders)
                {
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Time)
                        .FirstOrDefault(status => status.Status == StatusOrder.SHIPPING);

                    if (latestStatus != null)
                    {
                        var latestShippingOrder = new OrderDto
                        {
                            Id = order.Id,
                            Address = order.Address,
                            PhoneNumber = order.PhoneNumber,
                            Note = order.Note,
                            Paymethod = order.Paymethod,
                            Amount = order.Amount,
                            CustomerName = order.CustomerName,
                            DateCreated = order.DateCreated,
                            PassivedDate = order.PassivedDate,
                            ModifiedDate = order.ModifiedDate,
                            UserId = order.UserId,
                            EmployeeId = order.EmployeeId,
                            VoucherId = order.VoucherId,
                            OrderStatuses = new List<OrderStatusDto> { latestStatus },
                            OrderItems = order.OrderItems.Select(item => new OrderItemDto
                            {
                                OrderId = item.OrderId,
                                ProductId = item.ProductId,
                                ColorId = item.ColorId,
                                SizeId = item.SizeId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            }).ToList()
                        };

                        latestShippingOrders.Add(latestShippingOrder);
                    }
                }

                if (!latestShippingOrders.Any())
                {
                    return NotFound("No latest shipping orders found.");
                }

                return Ok(latestShippingOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetLatestDeliveriedOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestDeliveriedOrders()
        {
            try
            {
                var deliveriedOrders = await _service.OrderService.GetAllOrderAsync();

                if (deliveriedOrders == null || !deliveriedOrders.Any())
                {
                    return NotFound();
                }

                var latestDeliveriedOrders = new List<OrderDto>();

                foreach (var order in deliveriedOrders)
                {
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Time)
                        .FirstOrDefault(status => status.Status == StatusOrder.DELIVERIED);

                    if (latestStatus != null)
                    {
                        var latestDeliveriedOrder = new OrderDto
                        {
                            Id = order.Id,
                            Address = order.Address,
                            PhoneNumber = order.PhoneNumber,
                            Note = order.Note,
                            Paymethod = order.Paymethod,
                            Amount = order.Amount,
                            CustomerName = order.CustomerName,
                            DateCreated = order.DateCreated,
                            PassivedDate = order.PassivedDate,
                            ModifiedDate = order.ModifiedDate,
                            UserId = order.UserId,
                            EmployeeId = order.EmployeeId,
                            VoucherId = order.VoucherId,
                            OrderStatuses = new List<OrderStatusDto> { latestStatus },
                            OrderItems = order.OrderItems.Select(item => new OrderItemDto
                            {
                                OrderId = item.OrderId,
                                ProductId = item.ProductId,
                                ColorId = item.ColorId,
                                SizeId = item.SizeId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            }).ToList()
                        };

                        latestDeliveriedOrders.Add(latestDeliveriedOrder);
                    }
                }

                if (!latestDeliveriedOrders.Any())
                {
                    return NotFound("No latest deliveried orders found.");
                }

                return Ok(latestDeliveriedOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetLatestCANCELEDOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestCANCELEDOrders()
        {
            try
            {
                var declineOrders = await _service.OrderService.GetAllOrderAsync();

                if (declineOrders == null || !declineOrders.Any())
                {
                    return NotFound();
                }

                var latestDeclineOrders = new List<OrderDto>();

                foreach (var order in declineOrders)
                {
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Time)
                        .FirstOrDefault(status => status.Status == StatusOrder.CANCELED);

                    if (latestStatus != null)
                    {
                        var latestDeclineOrder = new OrderDto
                        {
                            Id = order.Id,
                            Address = order.Address,
                            PhoneNumber = order.PhoneNumber,
                            Note = order.Note,
                            Paymethod = order.Paymethod,
                            Amount = order.Amount,
                            CustomerName = order.CustomerName,
                            DateCreated = order.DateCreated,
                            PassivedDate = order.PassivedDate,
                            ModifiedDate = order.ModifiedDate,
                            UserId = order.UserId,
                            EmployeeId = order.EmployeeId,
                            VoucherId = order.VoucherId,
                            OrderStatuses = new List<OrderStatusDto> { latestStatus },
                            OrderItems = order.OrderItems.Select(item => new OrderItemDto
                            {
                                OrderId = item.OrderId,
                                ProductId = item.ProductId,
                                ColorId = item.ColorId,
                                SizeId = item.SizeId,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            }).ToList()
                        };

                        latestDeclineOrders.Add(latestDeclineOrder);
                    }
                }

                if (!latestDeclineOrders.Any())
                {
                    return NotFound("No latest decline orders found.");
                }

                return Ok(latestDeclineOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost("pay")]
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