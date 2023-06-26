using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
  [Table("Orders")]
  public class Order
  {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public int Status { get; set; } = ((int)OrderStatus.Confirm);
    public string Note { get; set; }
    public int Paymethod { get; set; } = ((int)PayMethod.Cash);
    public double Amount { get; set; }
    public string CustomerName { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime PassivedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public string UserId { get; set; }
    public AppUser AppUser { get; set; }
    public string VoucherId { get; set; }
    public Voucher Voucher { get; set; }

    public virtual IEnumerable<OrderItem> OrderItems { get; set; }
  }
}