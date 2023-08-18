using EntitiesDto.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Ultilities
{
    public static class UploadService
    {
        public static Dictionary<string, Dictionary<string, bool>> UploadImages(IEnumerable<ColorAPI> colors, string productId)
        {
            var urlList = new Dictionary<string, Dictionary<string, bool>>();
            var uploadSource = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", productId);
            
            try
            {
                foreach (var color in colors)
                {
                    urlList.Add(color.Id, new Dictionary<string, bool>());
                    var uploadPath = Path.Combine(uploadSource, color.Id);
                    foreach (var image in color.Images)
                    {
                        var extension = Path.GetExtension(image.Image.FileName);
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(uploadPath, fileName);

                        if (image.Image.Length > 0)
                        {
                            if (!Directory.Exists(uploadPath))
                            {
                                Directory.CreateDirectory(uploadPath);
                            }
                            using (var fileStream = System.IO.File.Create(filePath))
                            {
                                image.Image.CopyTo(fileStream);
                                fileStream.Flush();
                            };
                            var path = Path.Combine("Uploads", productId, color.Id, fileName);
                            urlList[color.Id].Add(path,image.SetAsDefault);
                        }
                    }
                    
                }
                return urlList;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
