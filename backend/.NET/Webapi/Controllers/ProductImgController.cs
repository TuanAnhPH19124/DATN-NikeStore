using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Ultilities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImgController : ControllerBase
    {
        [HttpPost("{productId}")]
        public async Task<ActionResult> UploadImage(string productId, IFormFile image)
        {
            try
            {
                if (image == null)
                {
                    return BadRequest("No image uploaded.");
                }

                // Gọi hàm UploadImages để tải lên ảnh
                UploadService.UploadImages(image, productId);

                return Ok("Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
