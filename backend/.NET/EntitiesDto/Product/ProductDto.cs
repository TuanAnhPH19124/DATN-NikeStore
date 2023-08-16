using Domain.Entities;
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
            public string Id { get; set; }
            public string Name { get; set; }
            public double RetailPrice { get; set; }
            public double CostPrice { get; set; }
            public string Description { get; set; }
            public Brands Brand { get; set; }
            public int DiscountRate { get; set; } = 1;
            public Status Status { get; set; }
            public int SoleId { get; set; }
            public int MaterialId { get; set; }
        // Other properties as needed


    }
}
