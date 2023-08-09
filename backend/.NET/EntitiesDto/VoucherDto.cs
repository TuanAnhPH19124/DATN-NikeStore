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
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
