using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class VoucherDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public double Expression { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(10);
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
