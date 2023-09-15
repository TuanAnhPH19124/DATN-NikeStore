using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class WishListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double DiscountRate { get; set; }
        public double RetailPrice { get; set; }
        public string ImgUrl { get; set; }
    }

    public class ProductForWLDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double DiscountRate { get; set; }
        public double RetailPrice { get; set; }
        public string ImgUrl { get; set; }
    }
}