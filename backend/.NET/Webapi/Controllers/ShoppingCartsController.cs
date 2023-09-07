using Domain.Entities;
using EntitiesDto;
using EntitiesDto.Datas;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Persistence;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly AppDbContext _dbContext;

        public ShoppingCartsController(IServiceManager serviceManager, AppDbContext dbContext)
        {
            _serviceManager=serviceManager;
            _dbContext=dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Id người dụng bị mất, yêu cầu cung cấp id người dùng" });
            }
            var cartItems = await _serviceManager.ShoppingCartItemsService.GetByUserId(id);
            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ShoppingCartItemAPI item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _serviceManager.ShoppingCartItemsService.AddToCart(item);
                    transaction.Commit();
                    return Ok(item);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { Error = ex.Message });
                    throw;
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody]ShoppingCartItems item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != item.Id)
                return BadRequest(new { Error = "Id không hợp lệ: ID cung cấp không khớp Id của giỏ hàng." });

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _serviceManager.ShoppingCartItemsService.UpdateQuantity(item);
                    transaction.Commit();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { Error = ex.Message });
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { Error = "Không tìm thấy Id giỏ hàng, yêu cầu Id giỏ hàng." });

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _serviceManager.ShoppingCartItemsService.DeleteCart(id);
                    transaction.Commit();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { Error = ex.Message });
                    throw;
                }
            }
        }


        [HttpDelete("clear/{id}")]
        public async Task<ActionResult> DeleteRange(string id){
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { Error = "Không tìm thấy Id giỏ hàng, yêu cầu Id giỏ hàng." });

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _serviceManager.ShoppingCartItemsService.ClearCart(id);
                    transaction.Commit();
                    return NoContent();
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                    throw;
                }
            }
        }
    }
}
