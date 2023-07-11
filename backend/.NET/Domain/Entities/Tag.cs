using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Tags")]
    public class Tag : BaseEntity
    {
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
