using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Orders")]
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public int Paymethod { get; set; } 
        public double Amount { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime PassivedDate { get; set; } 
        public DateTime ModifiedDate { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string VoucherId { get; set; }
        public string AddressId { get; set; }



        public AppUser AppUser { get; set; }    
        public Voucher Voucher { get; set; }
        public virtual Address address { get; set; }
        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
        public virtual List<OrderStatus> OrderStatuses { get; set; }
    }
}