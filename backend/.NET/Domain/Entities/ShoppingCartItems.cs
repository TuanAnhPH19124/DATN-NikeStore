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
        public string StockId { get; set; }
        public string AppUserId { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
