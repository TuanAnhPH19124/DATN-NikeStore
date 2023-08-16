using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EntitiesDto.Product
{
    public class ProductAPI
    {
        public string Name { get; set; }
        public double RetailPrice { get; set; }
        public double CostPrice { get; set; }
        public string Description { get; set; }
        public Brands Brand { get; set; }
        public int DiscountRate { get; set; } = 1;
        public Status Status { get; set; }
        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public List<ColorAPI> Colors { get; set; }
    }

    public class ColorAPI
    {
        public string Id { get; set; }
        public List<ImageAPI> Images { get; set; }
        public List<SizeAPI> Sizes { get; set; }

    }
   
    public class SizeAPI
    {
        public string Id { get; set; }
        public int UnitInStock { get; set; }
    }

    public class ImageAPI
    {
        public IFormFile Image { get; set; }
        public bool SetAsDefault { get; set; }
    }
}
