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
        [JsonIgnore]
        public string BarCode { get; set; }
        [JsonIgnore]
        public double CostPrice { get; set; } 
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Brands Brand { get; set; } = Brands.Nike;    
        public Status Status { get; set; }
        public int DiscountRate { get; set; } = 1;
        public virtual List<Category> Categories { get; set; } = new List<Category>();
        public virtual List<Tag> Tags { get; set; } = new List<Tag>();
        public virtual List<Stock> Stocks { get; set; } = new List<Stock>();
        public virtual List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public virtual IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ShoppingCartItems ShoppingCartItems { get; set; }
        public ICollection<ProductRate> ProductRate { get; set; }
        public virtual List<CategoryProduct> CategoryProducts { get; set; }
    }
}
