using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Colors")]
    public class Color 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Tên màu là bắt buộc")]
        public string Name { get; set; }

        public virtual List<Stock> Stocks { get; set; } = new List<Stock>();
        public virtual List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
