using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "Căn cước công dân là bắt buộc")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Căn cước phải là số")]
        public string SNN { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Ngày tháng năm sinh là bắt buộc")]
        public DateTime DateOfBirth { get; set; }

        public int Gender { get; set; }

        [Required(ErrorMessage = "Địa chỉ liên hệ là bắt buộc")]
        public string Address { get; set; }
    }
}
