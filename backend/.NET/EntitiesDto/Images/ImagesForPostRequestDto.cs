using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Images
{
    public class ImagesForPostRequestDto
    {
        public IFormFile Images { get; set; }
        public string ColorId { get; set; }
    }
}
