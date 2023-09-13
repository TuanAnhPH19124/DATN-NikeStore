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
        public async Task<ActionResult> GetbyUserId(string userId, [FromQuery]int? type = null){
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new {error = "Id không hợp lệ"});

            var result = await Task.Run(() =>{
                var orders = from o in _dbContext.Orders
                         join oi in _dbContext.OrderItems on o.Id equals oi.OrderId
                         join p in _dbContext.Products on oi.ProductId equals p.Id
                         join s in _dbContext.Sizes on oi.SizeId equals s.Id
                         join c in _dbContext.Colors on oi.ColorId equals c.Id
                         join pi in _dbContext.ProductImages on p.Id equals pi.ProductId
                         where o.UserId == userId && pi.ColorId == oi.ColorId && (type != null ? (int)o.CurrentStatus == type : true)
                
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

            var final = groupAndDistrictOrder.GroupBy(order => new{order.orderid, order.Status}).Select(group => new {
                orderId = group.Key.orderid,
                status = group.Key.Status,
                orderItems = group.ToList()
            }).ToList();

            return final;
            });
            
            return Ok(result);
        }


        [HttpGet("getOrderDetail/{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest(new {error = "Id không hợp lệ"});
            
            var result = await Task.Run(() =>{
                var order = from o in _dbContext.Orders
                            join oi in _dbContext.OrderItems on o.Id equals oi.OrderId
                            join os in _dbContext.OrderStatuses on o.Id equals os.OrderId
                            join p in _dbContext.Products on oi.ProductId equals p.Id
                            join c in _dbContext.Colors on oi.ColorId equals c.Id
                            join s in _dbContext.Sizes on oi.SizeId equals s.Id
                            join pi in _dbContext.ProductImages on p.Id equals pi.ProductId
                            join a in _dbContext.Addresses on o.AddressId equals a.Id
                            join v in _dbContext.Vouchers on o.VoucherId equals v.Id into voucherGroup
                            from voucher in voucherGroup.DefaultIfEmpty()
                            where o.Id == id && pi.ColorId == oi.ColorId
                            select new {
                                id = o.Id,
                                status = o.CurrentStatus,
                                payMethod = o.Paymethod,
                                voucherValue = voucher != null ? voucher.Value : 0,
                                orderItemId = oi.Id,
                                colorId = oi.ColorId,
                                sizeId = oi.SizeId,
                                productId = p.Id,
                                productName = p.Name,
                                discountRate = p.DiscountRate,
                                retailPrice = p.RetailPrice,
                                quantity = oi.Quantity,
                                color = c.Name,
                                size = s.NumberSize,
                                imgUrl = pi.ImageUrl,
                                orderStatusId = os.Id,
                                orderStatusStatus = os.Status,
                                orderStatusNote = os.Note,
                                orderStatusTime = os.Time,
                                CustomerName = a.FullName,
                                PhoneNumber = a.PhoneNumber,
                                AddressLine = a.AddressLine,
                                ProvinceId = a.CityCode,
                                DistrictId = a.ProvinceCode,
                                WardCode = a.WardCode
                            };
                var orderGroupByImage = order.ToList().GroupBy(order => new{
                    order.id,
                    order.status,
                    order.voucherValue,
                    order.payMethod,
                    order.orderItemId,
                    order.colorId,
                    order.sizeId,
                    order.productId,
                    order.productName, 
                    order.discountRate,
                    order.retailPrice, 
                    order.quantity,
                    order.color ,
                    order.size ,
                    order.orderStatusId ,
                    order.orderStatusStatus ,
                    order.orderStatusNote ,
                    order.orderStatusTime,
                    order.CustomerName,
                    order.PhoneNumber,
                    order.AddressLine,
                    order.ProvinceId,
                    order.DistrictId,
                    order.WardCode
                }).Select(group => new {
                    id = group.Key.id,
                    status = group.Key.status,
                    payMethod = group.Key.payMethod,
                    voucherValue = group.Key.voucherValue,
                    CustomerName = group.Key.CustomerName,
                    PhoneNumber = group.Key.PhoneNumber,
                    AddressLine = group.Key.AddressLine,
                    ProvinceId = group.Key.ProvinceId,
                    DistrictId = group.Key.DistrictId,
                    WardCode = group.Key.WardCode,

                    orderItemId = group.Key.orderItemId,
                    colorId = group.Key.colorId,
                    sizeId = group.Key.sizeId,
                    productId = group.Key.productId,
                    productName = group.Key.productName,
                    discountRate = group.Key.discountRate,
                    retailPrice = group.Key.retailPrice,
                    quantity = group.Key.quantity,
                    color = group.Key.color,
                    size = group.Key.size,
                    orderStatusId = group.Key.orderStatusId,
                    orderStatusStatus = group.Key.orderStatusStatus,
                    orderStatusNote = group.Key.orderStatusNote,
                    orderStatusTime = group.Key.orderStatusTime,
                    imgUrl = group.First().imgUrl
                    
                });

                var orderGroupByOrderItem = orderGroupByImage.ToList().GroupBy(order => new {
                    order.id,
                    order.status,
                    order.payMethod,
                    order.voucherValue,
                    order.CustomerName,
                    order.PhoneNumber,
                    order.AddressLine,
                    order.ProvinceId,
                    order.DistrictId,
                    order.WardCode,
                    order.orderStatusId,
                    order.orderStatusStatus,
                    order.orderStatusNote,
                    order.orderStatusTime
                }).Select(group => new{
                    id = group.Key.id,
                    status = group.Key.status,
                    payMethod = group.Key.payMethod,
                    voucherValue = group.Key.voucherValue,
                    CustomerName = group.Key.CustomerName,
                    PhoneNumber = group.Key.PhoneNumber,
                    AddressLine = group.Key.AddressLine,
                    ProvinceId = group.Key.ProvinceId,
                    DistrictId = group.Key.DistrictId,
                    WardCode = group.Key.WardCode,
                    orderStatusId = group.Key.orderStatusId,
                    orderStatusStatus = group.Key.orderStatusStatus,
                    orderStatusNote = group.Key.orderStatusNote,
                    orderStatusTime = group.Key.orderStatusTime,
                    orderItems = group.Select(item => new {
                        orderItemId = item.orderItemId,
                        colorId = item.colorId,
                        sizeId = item.sizeId,
                        productId = item.productId,
                        productName = item.productName,
                        discountRate = item.discountRate,
                        retailPrice = item.retailPrice,
                        quantity = item.quantity,
                        color = item.color,
                        size = item.size,
                        imgUrl = item.imgUrl
                    })
                }).ToList();

                var finalResult = orderGroupByOrderItem.GroupBy(order => new {
                    order.id,
                    order.status,
                    order.voucherValue,
                    order.payMethod,
                    order.CustomerName,
                    order.PhoneNumber,
                    order.AddressLine,
                    order.ProvinceId,
                    order.DistrictId,
                    order.WardCode,
                }).Select(g => new {
                    id = g.Key.id,
                    status = g.Key.status,
                    voucherValue = g.Key.voucherValue,
                    payMethod = g.Key.payMethod,
                    CustomerName = g.Key.CustomerName,
                    PhoneNumber = g.Key.PhoneNumber,
                    AddressLine = g.Key.AddressLine,
                    ProvinceId = g.Key.ProvinceId,
                    DistrictId = g.Key.DistrictId,
                    WardCode = g.Key.WardCode,
                    orderItems = g.First().orderItems,
                    orderStatuses = g.Select(i => new {
                        id = i.orderStatusId,
                        status = i.orderStatusStatus,
                        note = i.orderStatusNote,
                        time = i.orderStatusTime
                    }).ToList()
                });
                
                return finalResult;
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
                var finalResult = confirmedOrderStatuses.Where((p=>p.CurrentStatus==StatusOrder.CONFIRM));

                return Ok(finalResult);
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
                var finalResult = confirmedOrderStatuses.Where((p => p.CurrentStatus == StatusOrder.PENDING_SHIP));

                return Ok(finalResult);
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
                var finalResult = confirmedOrderStatuses.Where((p => p.CurrentStatus == StatusOrder.SHIPPING));

                return Ok(finalResult);
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
                var finalResult = confirmedOrderStatuses.Where((p => p.CurrentStatus == StatusOrder.DELIVERIED));

                return Ok(finalResult);
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
                var finalResult = confirmedOrderStatuses.Where((p => p.CurrentStatus == StatusOrder.CANCELED));

                return Ok(finalResult);
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
                        AppUserFullName = o.AppUser.FullName, // Lấy FullName từ AppUser
                        AddressLine = o.address.AddressLine, // Lấy AddressLine từ Address
                        FullName = o.address.FullName,
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