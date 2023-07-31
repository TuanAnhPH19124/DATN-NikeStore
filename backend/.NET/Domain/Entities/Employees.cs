using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employees
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Căn cước là bắt buộc")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Căn cước phải là số")]
        public string SNN { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại phải là số")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Ngày chỉnh sửa là bắt buộc")]
        public DateTime ModifiedDate { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        public string Role { get; set; }

        public bool Status { get; set; }       
    }
}
