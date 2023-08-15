using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductMaterial
    {
        public int MaterialId { get; set; }
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Material Material { get; set; }
    }
}
