using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOConvert
{
    public class AppUserForCreationDTO
    {
        [Required(ErrorMessage = "Mật khâu không được để trống!")]
        [StringLength(12, ErrorMessage = "Mật khẩu không được dài quá 12 kí tụ")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@#$%^&+=!]).*$", ErrorMessage = " Mật khẩu phải chữa ít nhất 1 chữ số (0-9), 1 kí tự (a-z) và 1 kí tự đặc biệt (*, @,...).")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username không được để trống!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Username không được để trống!")]
        [MaxLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 số!")]
        public string PhoneNumber { get; set; }
    }
}
