using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

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

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; }

        public bool Status { get; set; } 

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
