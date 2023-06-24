using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class GetProductByCategoryRequestDto
    {
        public string Id { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public int Brand { get; set; }
        public int DiscountRate { get; set; }
        public string Name { get; set; }
    }
}
