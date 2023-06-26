using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using EntitiesDto.Order;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Abstractions;

namespace Webapi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : Controller
  {
    private readonly ILogger<OrdersController> _logger;
    private readonly IServiceManager _service;
    public OrdersController(ILogger<OrdersController> logger)
    {
      _logger = logger;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Payment([FromBody] OrderPostRequestDto orderDto)
    {
      var order = orderDto.Adapt<Order>();
      if (order.Paymethod == ((int)PayMethod.Vnpay))
      {
        #region Thanh toán vnpay


        #endregion
        return Ok();
      }
      #region Đẩy dữ liệu vào db
      try
      {
        await _service.OrderService.PostAndSendNontification(order);
        return Ok();
      }
      catch (System.Exception)
      {
        throw;
      }
      #endregion
    }


  }
}