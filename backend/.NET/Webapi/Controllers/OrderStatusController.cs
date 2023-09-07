using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Abstractions;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly AppDbContext _context;

        public OrderStatusController(IServiceManager serviceManager, AppDbContext context)
        {
            _serviceManager = serviceManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(OrderStatusDto orderStatusDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Chuyển đổi OrderStatusDto thành OrderStatus
                    var orderStatus = new OrderStatus
                    {
                      
                        OrderId = orderStatusDto.OrderId,
                        Status = orderStatusDto.Status,
                        Time = orderStatusDto.Time,
                        Note = orderStatusDto.Note,
                    };

                    await _serviceManager.OrderStatusService.AddAsync(orderStatus);
                    transaction.Commit();
                    return Ok(orderStatus);
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    return BadRequest();
                }
            }
        }

    }
}
