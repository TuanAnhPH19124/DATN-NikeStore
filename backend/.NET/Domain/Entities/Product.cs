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
        public double CostPrice { get; set; } 
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; } = Status.ACTIVE;
        public Brands Brand { get; set; } = Brands.Nike;
        public int DiscountRate { get; set; } = 1;
        public virtual IEnumerable<ShoppingCartItems> ShoppingCartItems { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Tag> Tags { get; set; } 
        public virtual List<Stock> Stocks { get; set; } 
        public virtual List<ProductImage> ProductImages { get; set; } 
        public virtual IEnumerable<OrderItem> OrderItems { get; set; } 
        public IEnumerable<ProductRate> ProductRate { get; set; }
        public virtual List<CategoryProduct> CategoryProducts { get; set; }
    }
}
