using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public class WishListPost
        {
            [Required]
            public string ProductsId { get; set; }
            [Required]
            public string AppUserId { get; set; }
        }
    }
}