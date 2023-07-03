using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {

        public string BarCode { get; set; }
        public double CostPrice { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Brands Brand { get; set; } = Brands.Nike;
        public int DiscountRate { get; set; } = 1;

        public virtual List<Category> Categories { get; set; } = new List<Category>();
        public virtual List<Tag> Tags { get; set; } = new List<Tag>();
        public virtual List<Stock> Stocks { get; set; } = new List<Stock>();
        public virtual List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ShoppingCartItems ShoppingCartItems { get; set; }
    }
}
