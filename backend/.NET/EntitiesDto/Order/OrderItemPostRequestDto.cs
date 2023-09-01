using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto.Order
{
    public class OrderItemPostRequestDto
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ColorId { get; set; }
        [Required]
        public string SizeId { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}