using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductDtoForGet
    {
        public string Name { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public int DiscountRate { get; set; } = 1;
        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public List<ColorDtoForGet> Colors { get; set; }
        public List<CategoryDtoForGet> Categories { get; set; }
    }

    public class ColorDtoForGet
    {
        public string Id { get; set; }
        public List<ImageDtoForGet> Images { get; set; }
        public List<SizeDtoForGet> Sizes { get; set; }
    }

    public class ImageDtoForGet
    {
        public string ImageUrl { get; set; }
        public bool SetAsDefault { get; set; }
    }

    public class SizeDtoForGet
    {
        public string Id { get; set; }
        public int UnitInStock { get; set; }
    }

    public class CategoryDtoForGet
    {
        public string Id { get; set; }
    }
}
