using System;
using System.Collections.Generic;
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
        public int NumberSize { get; set; }
        public string Description { get; set; }

        public virtual List<Stock> Stocks { get; set; } 
    }
}
