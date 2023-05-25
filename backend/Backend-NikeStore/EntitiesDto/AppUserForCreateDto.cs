using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class AppUserForCreateDto
    {
        [Required(ErrorMessage = "Không được bỏ trống tên đăng nhập.")]
        [MaxLength(50, ErrorMessage = "Tên đăng nhập không được nhiều hơn 50 kí tự.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống Email.")]
        [MaxLength(50, ErrorMessage = "Email không được nhiều hơn 50 kí tự.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(\+[a-zA-Z0-9._%+-]+)?@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống mật khẩu.")]
        [MaxLength(12, ErrorMessage = "Mật khẩu không được nhiều hơn 50 kí tự.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^\w\d\s])\S{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất một chữ cái [a-z], một số [0-9] và một kí tự đặc biệt * @")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống số điện thoại.")]
        [MaxLength(10, ErrorMessage = "Số điện thoại không được nhiều hơn 10 số.")]
        public string PhoneNumber { get; set; }
    }
}
