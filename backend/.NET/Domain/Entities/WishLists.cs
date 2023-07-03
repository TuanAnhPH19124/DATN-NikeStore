using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public  class WishLists
    {
        [Key]
        public string ProductsId { get; set; }

        [Key]
        public string AppUserId { get; set; }

        [ForeignKey("ProductsId")]
        public Product Product { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
