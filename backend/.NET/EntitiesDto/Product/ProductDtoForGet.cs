using Domain.Enums;
using EntitiesDto.Images;
using EntitiesDto.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductDtoForGet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public int DiscountRate { get; set; }

        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public List<StockDto> Stocks { get; set; }
        public List<CategoryProductDto> CategoryProducts { get; set; }
        public List<ProductImageDto> ProductImages { get; set; }
    }



   
}
