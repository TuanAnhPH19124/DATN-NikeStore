using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Voucher
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
