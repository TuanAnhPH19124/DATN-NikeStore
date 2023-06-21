using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingCarts
    {
     
            [Key]
            public Guid Id { get; set; }

            [ForeignKey("AppUser")]
            public string AppUserId { get; set; }

            // Các thuộc tính khác của ShoppingCart

            public AppUser AppUser { get; set; }
            public ICollection<ShoppingCartItems> Items { get; set; }

    }
}
