using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public int Paymethod { get; set; }
        public double Amount { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime PassivedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string VoucherId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ColorId { get; set; }
        public string SizeId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
