using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductForUpdateProductDto
    {
        public string Name { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Brands Brand { get; set; }
        public Status Status { get; set; }
        public int DiscountRate { get; set; }

        public List<StockDto> Stocks { get; set; } = new List<StockDto>();
    }
}
