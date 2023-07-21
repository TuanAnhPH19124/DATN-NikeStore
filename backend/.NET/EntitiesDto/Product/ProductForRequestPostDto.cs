using Domain.Entities;
using Domain.Enums;
using EntitiesDto.Images;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntitiesDto.Product
{
    public class ProductForRequestPostDto : BaseEntity
    {
        public string BarCode { get; set; }     
        public double CostPrice { get; set; }      
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Brands Brand { get; set; }   
        public int DiscountRate { get; set; }
        public IEnumerable<ImagesForPostRequestDto> Images { get; set; }
    }
}
