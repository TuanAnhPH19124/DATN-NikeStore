using Domain.Entities;
using EntitiesDto.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImagesController(IWebHostEnvironment environment)
        {
            _environment=environment;
        }

        [HttpPost("Upload")]
        public IActionResult Updload([FromForm] ProductForPostWithImagesDto images)
        {
            try
            {
                foreach (var item in images.Images)
                {
                    var extension = Path.GetExtension(item.FileName);
                    var fileName = "productImage" + Guid.NewGuid().ToString() + extension;
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload");
                    var filePath = Path.Combine(uploadPath, fileName);

                    if (item.Length > 0)
                    {
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }
                        using (var fileStream = System.IO.File.Create(filePath))
                        {
                            item.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return Ok(images);
        }

    }
}
