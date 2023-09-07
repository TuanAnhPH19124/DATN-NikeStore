using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto.Order
{
    public class OrderPostRequestDto
    {
        public string Address { get; set; } = null;
        public string PhoneNumber { get; set; } = null;
        public string CustomerName { get; set; } = null;
        public string Note { get; set; } = null;
        public string UserId { get; set; } = null;
        public string EmployeeId { get; set; } = null;
        public string VoucherId { get; set; } = null;
        public string AddressId { get; set; } = null;
      
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int PaymentMethod { get; set; }
        [Required]
        public double Amount { get; set; }
        public List<OrderItemPostRequestDto> OrderItems { get; set; }
    
    }

    public class OrderAtStorePostRequestDto  : OrderPostRequestDto
    {
        [Required(ErrorMessage = "Yêu cầu Shipping để tiếp nhận vận chuyển")]
        public bool Shipping { get; set; } 
    }
}