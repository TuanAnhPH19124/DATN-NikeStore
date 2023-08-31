using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderStatus
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; }
        public Domain.Enums.StatusOrder Status { get; set; }
        public DateTime Time { get; set; }
        public string Note { get; set; }

        public Order Order { get; set; }
    }
}
