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

        [HttpGet("GetLatestConfirmedOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestConfirmedOrders()
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
                    // Lấy trạng thái có giá trị int lớn nhất cho mỗi đơn hàng
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Status)
                        .FirstOrDefault();

                    if (latestStatus != null && latestStatus.Status == StatusOrder.CONFIRM)
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
                var confirmedOrderStatuses = await _service.OrderService.GetAllOrderAsync();

                if (confirmedOrderStatuses == null || !confirmedOrderStatuses.Any())
                {
                    return NotFound();
                }

                var confirmedOrdersWithLatestStatus = new List<OrderDto>();

                foreach (var order in confirmedOrderStatuses)
                {
                    // Lấy trạng thái có giá trị int lớn nhất cho mỗi đơn hàng
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Status)
                        .FirstOrDefault();

                    if (latestStatus != null && latestStatus.Status == StatusOrder.PENDING_SHIP)
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

        [HttpGet("GetLatestShippingOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestShippingOrders()
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
                    // Lấy trạng thái có giá trị int lớn nhất cho mỗi đơn hàng
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Status)
                        .FirstOrDefault();

                    if (latestStatus != null && latestStatus.Status == StatusOrder.SHIPPING)
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

        [HttpGet("GetLatestDeliveredOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestDeliveredOrders()
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
                    // Lấy trạng thái có giá trị int lớn nhất cho mỗi đơn hàng
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Status)
                        .FirstOrDefault();

                    if (latestStatus != null && latestStatus.Status == StatusOrder.DELIVERIED)
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

        [HttpGet("GetLatestCancelOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetLatestDeclineOrders()
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
                    // Lấy trạng thái có giá trị int lớn nhất cho mỗi đơn hàng
                    var latestStatus = order.OrderStatuses
                        .OrderByDescending(status => status.Status)
                        .FirstOrDefault();

                    if (latestStatus != null && latestStatus.Status == StatusOrder.CANCELED)
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