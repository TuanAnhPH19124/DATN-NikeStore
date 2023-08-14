using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductSole
    {
        public int SoleId { get; set; }
        public string ProductId { get; set; }

        public virtual Sole Sole { get; set; }
        public virtual Product Product { get; set; }
    }
}
