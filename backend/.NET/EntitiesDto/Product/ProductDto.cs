using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace EntitiesDto.Product
{
    public class ProductAPI
    {
        public string Name { get; set; }
        public double RetailPrice { get; set; }
     
        public string Description { get; set; }
        public Brands Brand { get; set; }
        public int DiscountRate { get; set; }
        public DiscountType DiscountType { get; set; }  
        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public List<ColorAPI> Colors { get; set; }
        public List<CategoryAPI> Categories { get; set; }
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
    public class ProductUpdateAPI
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public int DiscountRate { get; set; }
        public DiscountType DiscountType { get; set; }

        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public List<CategoryAPI> Categories { get; set; }
        public List<ColorAPI> Colors { get; set; }
    }
    public class ProductFilterOptionAPI
    {
        public string Keyword { get; set; } = null;
        public List<string> Categories { get; set; } = null;
        public List<int> Genders { get; set; } = null;
        public List<string> Sizes { get; set; } = null;
        public List<string> Colors { get; set; } = null;
        public double Min { get; set; }
        public double Max { get; set; }
        public SortBy? SortBy { get; set; } = null;
        public bool? Sale { get; set; } = null;
    }
    public class GenderAPI
    {
        public Gender Id { get; set; }
    }
    public class SizeFilterAPI
    {
        public string Id { get; set; }
    }
    public class ColorFilterAPI
    {
        public string Id { get; set; }
    }
    public class CategoryAPI
    {
        public string Id { get; set; }
    }






}
