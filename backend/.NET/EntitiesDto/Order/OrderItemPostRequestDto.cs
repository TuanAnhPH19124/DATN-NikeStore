using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto.Order
{
    public class OrderItemPostRequestDto
    {
        public string ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
}