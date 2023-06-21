using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
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
            var shoppingCart = await _serviceManager.ShoppingCartsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return shoppingCart;
        }
        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddCartItem(string userId, ShoppingCartItems item)
        {
            var shoppingCart = await _serviceManager.ShoppingCartsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            item.ShoppingCartsId = shoppingCart.Id;

            await _serviceManager.ShoppingCartsService.AddCartItemAsync(item);

            return Ok();
        }
        [HttpPut("{userId}/items/{itemId}")]
        public async Task<IActionResult> UpdateCartItem(string userId, string productId, ShoppingCartItems item)
        {
            var shoppingCart = await _serviceManager.ShoppingCartsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            item.ShoppingCartsId = shoppingCart.Id;
            item.ProductsId = productId;

            await _serviceManager.ShoppingCartsService.UpdateCartItemAsync(item);

            return Ok();
        }
        [HttpDelete("{userId}/items/{productId}")]
        public async Task<IActionResult> DeleteCartItem(string userId, string productId)
        {
            var shoppingCart = await _serviceManager.ShoppingCartsService.GetByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            await _serviceManager.ShoppingCartsService.DeleteCartItemAsync(productId);

            return Ok();
        }
    }
}
