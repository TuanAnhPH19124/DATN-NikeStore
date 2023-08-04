using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
       
        [Table("CategoryProduct")]
        public class CategoryProduct
        {
            public string ProductId { get; set; }
            public Product Product { get; set; }

            public string CategoryId { get; set; }
            public Category Category { get; set; }
        }
}
    

