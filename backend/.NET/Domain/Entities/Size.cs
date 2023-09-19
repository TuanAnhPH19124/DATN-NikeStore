using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Sizes")]
    public class Size
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Số size là bắt buộc")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Size phải là số")]
        public int NumberSize { get; set; }
        public string Description { get; set; }

        public virtual List<Stock> Stocks { get; set; } 
        public virtual ICollection<ShoppingCartItems> ShoppingCartItems { get; set; }
    }
}
