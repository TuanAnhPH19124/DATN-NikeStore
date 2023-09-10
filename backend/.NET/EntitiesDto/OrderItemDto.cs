using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class OrderItemSelectDto
    {
        public string ProductId { get; set; }
        public string productName { get; set; }
        public int TotalQuantitySold { get; set; }
        public double TotalRevenue { get; set; }
    }
}
