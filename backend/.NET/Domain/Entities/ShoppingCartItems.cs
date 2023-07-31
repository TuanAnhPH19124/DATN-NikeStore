using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingCartItems
    {
        [Key]
        [Column(Order = 0)]
        public Guid ShoppingCartsId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string ProductsId { get; set; }

        public int Quantity { get; set; }

        // Các thuộc tính khác của ShoppingCartItem

        public ShoppingCarts ShoppingCarts { get; set; }
        public Product Product { get; set; }
    }
}
