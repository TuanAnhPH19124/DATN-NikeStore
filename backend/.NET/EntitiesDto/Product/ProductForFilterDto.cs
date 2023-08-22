using Domain.Enums;
using EntitiesDto.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductForFilterDto
    {
        public string BarCode { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; } = Status.ACTIVE;
        public Brands Brand { get; set; } = Brands.Nike;
        public int DiscountRate { get; set; } = 1;

        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public List<StockDto> Stocks { get; set; }
        public List<CategoryProductDto> CategoryProducts { get; set; }
    }
}
