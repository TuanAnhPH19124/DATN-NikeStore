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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; } = 1;
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCarts ShoppingCarts { get; set; }
    }
}
