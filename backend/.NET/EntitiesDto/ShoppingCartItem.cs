using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class ShoppingCartItemAPI
    {
        public string AppUserId { get; set; }
        public string StockId { get; set; }
        public int Quantity { get; set; }
    }

}
