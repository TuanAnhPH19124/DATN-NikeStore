using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class Dto
    {
        public class ShopppingCartPostDto
        {
            public string AppUserId { get; set; }
            public ShoppingCartItemsDto ShoppingCartItemsDto { get; set; }
        }

        public class ShoppingCartItemsDto 
        {
            public int Quantity { get; set; }
            public string ProductId { get; set; }
            public Boolean IsQuantity { get; set; }
        }

        public class WishListPost
        {
            [Required]
            public string ProductsId { get; set; }
            [Required]
            public string AppUserId { get; set; }
        }

        public class EmployeeDto
        {

            [Required(ErrorMessage = "Căn cước công dân là bắt buộc")]
            [RegularExpression(@"^\d+$", ErrorMessage = "Căn cước phải là số")]
            public string SNN { get; set; }

            [Required(ErrorMessage = "Họ và tên là bắt buộc")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
            [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại phải là số")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Ngày tháng năm sinh là bắt buộc")]
            public DateTime DateOfBirth { get; set; }

            public int Gender { get; set; }

            [Required(ErrorMessage = "Quê quán là bắt buộc")]
            public string HomeTown { get; set; }

            public bool Status { get; set; }
            public string AppUserId { get; set; }
        }

        public class UpdateEmployeeDto
        {
            public string Id { get; set; }

            [Required(ErrorMessage = "Căn cước công dân là bắt buộc")]
            [RegularExpression(@"^\d+$", ErrorMessage = "Căn cước phải là số")]
            public string SNN { get; set; }

            [Required(ErrorMessage = "Họ và tên là bắt buộc")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
            [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại phải là số")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Ngày tháng năm sinh là bắt buộc")]
            public DateTime DateOfBirth { get; set; }

            public int Gender { get; set; }

            [Required(ErrorMessage = "Quê quán là bắt buộc")]
            public string HomeTown { get; set; }

            public DateTime ModifiedDate { get; set; } = DateTime.Now;

            public bool Status { get; set; }
        }
    }
}