using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Stocks")]
    public class Stock
    {
        public string StockId { get; set; } = Guid.NewGuid().ToString();
        public int UnitInStock { get; set; }


        public string ProductId { get; set; }
        public string ColorId { get; set; }
        public string SizeId { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}
