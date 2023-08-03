using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductRate
    {

        [Key]
        [Column(Order = 0)]
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Product")]
        public string ProductId { get; set; }

        public int RateScore { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Review { get; set; }
        public string UserName { get; set; }
        public string Response { get; set; }

        public AppUser AppUser { get; set; }
        public Product Product { get; set; }


    }
}
