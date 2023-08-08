using Domain.Entities;
using EntitiesDto;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
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
        public async Task<ActionResult<ShoppingCarts>> GetShoppingCartByUserId(string userId)
        {
            var shoppingCart = await _serviceManager.ShoppingCartItemsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }
        [HttpPost]
        public async Task<IActionResult> AddCartItem([FromBody]Dto.ShopppingCartPostDto item)
        {
            var newCart = item.Adapt<ShoppingCarts>();
            await _serviceManager.ShoppingCartService.AddAsync(newCart);
            var newCartItem = item.ShoppingCartItemsDto.Adapt<ShoppingCartItems>();
            newCartItem.ShoppingCartId = newCart.Id;
            await _serviceManager.ShoppingCartItemsService.AddCartItemAsync(newCartItem);
            return Ok();
        }
        [HttpPut("{UserId}")]
        public async Task<IActionResult> UpdateCartItem(string UserId, ShoppingCartItems item)
        {
            var shoppingCart = await _serviceManager.ShoppingCartItemsService.GetByUserIdAsync(UserId);

            // if (shoppingCart == null)
            // {
            //     return NotFound();
            // }

            await _serviceManager.ShoppingCartItemsService.UpdateCartItemAsync(item);

            return Ok();
        }
        [HttpDelete("{userId}/items/{productId}")]
        public async Task<IActionResult> DeleteCartItem(string userId, string productId)
        {
            var shoppingCart = await _serviceManager.ShoppingCartItemsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            await _serviceManager.ShoppingCartItemsService.DeleteCartItemAsync(productId);

            return Ok();
        }
    }
}
