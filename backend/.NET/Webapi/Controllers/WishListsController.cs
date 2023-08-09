using Domain.Entities;
using EntitiesDto;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Net;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("api/wishlists")]
    public class WishListsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public WishListsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWishList([FromBody]Dto.WishListPost newWishDto)
        {
            if (!ModelState.IsValid){
                return BadRequest("Missing parameter!");
            }
            var newwish = newWishDto.Adapt<WishLists>();
            var checkExsist = await _serviceManager.WishListsService.IsWishListExists(newwish);
            if (!checkExsist){
                await _serviceManager.WishListsService.CreateAsync(newwish);
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.Found, "Sản phẩm này đã nằm trong mục yêu thích của bạn.");
        }

        [HttpDelete("{appUserId}/{productId}")]
        public async Task<IActionResult> DeleteWishList(string appUserId, string productId)
        {
            var deletedWishList = await _serviceManager.WishListsService.DeleteAsync(appUserId, productId);
            if (deletedWishList == null)
            {
                return NotFound();
            }
            return Ok(deletedWishList);
        }

        [HttpGet("{appUserId}")]
        public async Task<IActionResult> GetWishListsByAppUserId(string appUserId)
        {
            var wishLists = await _serviceManager.WishListsService.GetItemsByUserID(appUserId);
            return Ok(wishLists);
        }
    }

}
