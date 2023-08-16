using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ImageUrl { get; set; }
        public bool SetAsDefault { get; set; } = false;

        public string ProductId { get; set; }
        public string ColorId { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }

    }
}
