using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto.Order
{
    public class OrderConfirmPutRequestDto
    {
        [Required(ErrorMessage = "Yêu cầu trạng thái hóa đơn")]
        public int Status { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}