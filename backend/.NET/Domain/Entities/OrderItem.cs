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
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}