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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public IEnumerable<ShoppingCartItems> ShoppingCartItems { get; set; }

    }
}
