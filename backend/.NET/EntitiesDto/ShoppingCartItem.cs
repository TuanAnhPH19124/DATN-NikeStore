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
        public string ProductId { get; set; }
        public string ColorId { get; set; }
        public string SizeId { get; set; }
        public int Quantity { get; set; }
    }

    public class ShoppingCartItemPutAPI
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public string ColorId { get; set; }
        public string SizeId { get; set; }
    }
}
