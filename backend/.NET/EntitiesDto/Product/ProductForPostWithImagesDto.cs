using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductForPostWithImagesDto 
    {
        public IEnumerable<IFormFile> Images { get; set; }
        public ProductForThirdServiceDto Product { get; set; }
    }
}
