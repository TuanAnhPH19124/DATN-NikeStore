using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ShoppingCartDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeId { get; set; }
        public string ColorId { get; set; }
        public ShoppingCartProductDto Product { get; set; }
    }

    public class ShoppingCartProductDto
    {
        public string Id { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public double DiscountRate { get; set; }
        public double RetailPrice { get; set; }
    }
}
