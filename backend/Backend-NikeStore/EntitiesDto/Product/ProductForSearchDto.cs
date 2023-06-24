using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductForSearchDto : BaseEntity
    {
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Brands Brand { get; set; } = Brands.Nike;
        public int DiscountRate { get; set; } = 1;
    }
}
