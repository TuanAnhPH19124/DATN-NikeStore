using Domain.Entities;
using EntitiesDto;
using EntitiesDto.Datas;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
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

        public ShoppingCartsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Data.ShoppingCartItemData>>> GetShoppingCartByUserId(string userId)
        {
            var shoppingCart = await _serviceManager.ShoppingCartItemsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }

        [HttpPost]
        public async Task<IActionResult> AddCartItemTest([FromBody] Dto.ShopppingCartPostDto item)
        {

            var existingCart = await _serviceManager.ShoppingCartService.GetById(item.AppUserId);
            if (existingCart == false)
            {              
                var newCart = item.Adapt<ShoppingCarts>();
                await _serviceManager.ShoppingCartService.AddAsync(newCart);               
            }          
             var get = await _serviceManager.ShoppingCartService.GetShoppingCartIdByUserId(item.AppUserId);
             var newCartItem = item.ShoppingCartItemsDto.Adapt<ShoppingCartItems>();
             newCartItem.ShoppingCartId = get;
            var check = await _serviceManager.ShoppingCartItemsService.checkProduct(newCartItem.ProductId, get);
            if (check != null)
            {
                check.Quantity += newCartItem.Quantity;
                await _serviceManager.ShoppingCartItemsService.UpdateCartItemAsync(check.Id, check);
            }
            else
            {
                await _serviceManager.ShoppingCartItemsService.AddCartItemAsync(newCartItem);
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] Dto.ShoppingCartItemsDto shoppingCartItemsDto)
        {
            //await _serviceManager.ShoppingCartItemsService.UpdatePutAsync(shoppingCartItemsDto.ShoppingCartId, shoppingCartItemsDto.IsQuantity);
            return Ok();
        }

        [HttpDelete("{cartItemId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromCartItem(string cartItemId, string productId)
        {         
                await _serviceManager.ShoppingCartItemsService.RemoveProductFromCartItemAsync(cartItemId,productId);             
                return Ok();         
        }
    }
}
