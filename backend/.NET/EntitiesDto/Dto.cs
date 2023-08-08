using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public static class Dto
    {
        public class ShopppingCartPostDto
        {
            public string AppUserId { get; set; }
            public ShoppingCartItemsDto ShoppingCartItemsDto { get; set; }
        }

        public class ShoppingCartItemsDto 
        {
            public int Quantity { get; set; } = 1;
            public string ProductId { get; set; }
            public string ShoppingCartId { get; set; }
        }
    }
}