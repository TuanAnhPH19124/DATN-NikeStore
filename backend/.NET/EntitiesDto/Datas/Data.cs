using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto.Datas
{
    public class Data
    {
        public class ShoppingCartItemData
        {
            public string Id { get; set; }
            public ProductShoppingCartData Product { get; set; }
            public int Quantity { get; set; }
        }

        public class ProductShoppingCartData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public double RetailPrice { get; set; }
            public int DiscountRate { get; set; }
        }
    }
}
