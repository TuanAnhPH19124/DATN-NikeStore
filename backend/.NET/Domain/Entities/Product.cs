using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public string BarCode { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public int DiscountRate { get; set; } 
        public int SoleId { get; set; }
        public int MaterialId { get; set; }
        public DiscountType DiscountType { get; set; }

        public virtual Material Material { get; set; }
        public virtual Sole Sole { get; set; }
        public virtual List<Tag> Tags { get; set; } 
        public virtual List<Stock> Stocks { get; set; } 
        public virtual List<ProductImage> ProductImages { get; set; } 
        public virtual IEnumerable<OrderItem> OrderItems { get; set; } 
        public virtual IEnumerable<ProductRate> ProductRate { get; set; }
        public virtual List<CategoryProduct> CategoryProducts { get; set; }
        public virtual ICollection<ShoppingCartItems> ShoppingCartItems { get; set; }
    }
}
