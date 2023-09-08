using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Images
{
    public class ProductImageDto
    {
        public string ImageUrl { get; set; }
        public bool SetAsDefault { get; set; } = false;
        public string ColorId { get; set; }
    }
}
