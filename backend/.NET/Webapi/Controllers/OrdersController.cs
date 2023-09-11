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
using Nest;

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

        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult> GetbyUserId(string userId){
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new {error = "Id không hợp lệ"});

            var result = await Task.Run(() =>{
                var orders = from o in _dbContext.Orders
                         join oi in _dbContext.OrderItems on o.Id equals oi.OrderId
                         join p in _dbContext.Products on oi.ProductId equals p.Id
                         join s in _dbContext.Sizes on oi.SizeId equals s.Id
                         join c in _dbContext.Colors on oi.ColorId equals c.Id
                         join pi in _dbContext.ProductImages on p.Id equals pi.ProductId
                         where o.UserId == userId && pi.ColorId == oi.ColorId
                         select new
                         {
                            orderid= o.Id,
                            orderitemid = oi.Id,
                            ProductName = p.Name,
                            DiscountRate = p.DiscountRate,
                            RetailPrice = p.RetailPrice,
                            Quantity = oi.Quantity,
                            SizeNumber = s.NumberSize,
                            Color = c.Name,
                            ImgUrl = pi.ImageUrl,
                            Status = o.CurrentStatus,
                         };
          
            var groupAndDistrictOrder = orders.ToList().GroupBy(order => new{
                order.orderid,
                order.orderitemid,
                order.ProductName,
                order.DiscountRate,
                order.RetailPrice,
                order.Quantity,
                order.SizeNumber,
                order.Color,
                order.Status
            }).Select(group => new {
                orderid= group.Key.orderid,
                orderitemid = group.Key.orderitemid,
                ProductName = group.Key.ProductName,
                DiscountRate = group.Key.DiscountRate,
                RetailPrice = group.Key.RetailPrice,
                Quantity = group.Key.Quantity,
                SizeNumber = group.Key.SizeNumber,
                Color = group.Key.Color,
                Status = group.Key.Status,
                ImgUrl = group.First().ImgUrl
            });

            var final = groupAndDistrictOrder.GroupBy(order => order.orderid).Select(order => new {
                orderId = order.Key,
                orderItems = order.ToList()
            }).ToList();

            return final;
            });
            
            return Ok(result);
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
                            AddressLine = order.AddressLine,
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
                            AddressLine = order.AddressLine,
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
                            AddressLine = order.AddressLine,
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
                            AddressLine = order.AddressLine,
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
                            AddressLine = order.AddressLine,
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


        [HttpPost("payOnline")]
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
        [HttpGet("GetOrderByUserId")]
        public async Task<ActionResult<Order>> GetOrderByUserId(string userId,string orderId)
        {
            try
            {
                var order = await _dbContext.Orders
                    .Where(o => o.UserId == userId&&o.Id==orderId)
                    .Select(o => new
                    {
                        o.Id,
                        o.PhoneNumber,
                        o.Note,
                        o.Paymethod,
                        o.Amount,
                        o.CustomerName,
                        o.DateCreated,
                        o.PassivedDate,
                        o.ModifiedDate,
                        o.UserId,
                        o.EmployeeId,
                        o.VoucherId,
                        o.AddressId,
                        o.CurrentStatus,
                        AddressPhoneNumber = o.address.PhoneNumber, // Lấy PhoneNumber từ Address
                        AppUserFullName = o.AppUser.FullName // Lấy FullName từ AppUser
                    })
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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