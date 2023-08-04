using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductDto
    {
       
            public string Name { get; set; }
            public double RetailPrice { get; set; }
            public string Description { get; set; }
            public Brands Brand { get; set; }
            public double DiscountRate { get; set; }
            public Status Status { get; set; }
            // Other properties as needed
        

    }
}
