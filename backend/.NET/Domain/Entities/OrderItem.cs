using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("OrderItems")]
    public class OrderItem
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public Order Order { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public string ColorId { get; set; }
        public Color Color { get; set; }
        public string SizeId { get; set; }
        public Size Size { get; set; }
        public DateTime OrderDate { get; set; }


        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}