using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto.Order
{
    public class OrderAtStorePostRequestDto
    {
        public string UserId { get; set; } = null;
        public string Address { get; set; } = null;
        [MaxLength(10, ErrorMessage = "Số diện thoại không được nhiều hơn 10 số")]
        public string PhoneNumber { get; set; } = null;
        public string Note { get; set; } = null;
        public int PaymentMethod { get; set; }
        public double Amount { get; set; }
        //public string CustomerName { get; set; } = string.Empty;
        public string VoucherId { get; set; } = null;
        public List<OrderItemPostRequestDto> OrderItems { get; set; }
    
    }

    public class OrderPostRequestDto : OrderAtStorePostRequestDto
    {
        
    }
}