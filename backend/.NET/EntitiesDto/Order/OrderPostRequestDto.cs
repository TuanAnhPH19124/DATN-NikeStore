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
        [Required(ErrorMessage = "Thiếu Địa chỉ nhận hàng")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Thiếu số điện thoại")]
        [MaxLength(10, ErrorMessage = "Số diện thoại không được nhiều hơn 10 số")]
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string VoucherId { get; set; } = null;
        public List<OrderItemPostRequestDto> OrderItems { get; set; }
    }

    public class OrderPostRequestDto : OrderAtStorePostRequestDto
    {
        [Required]
        public string UserId { get; set; }
    }
}