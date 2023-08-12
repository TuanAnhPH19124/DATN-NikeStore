using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Schedule
    {
        public string ScheduleId { get; set; }

      
        public string EmployeeId { get; set; }
        public Employee Employees { get; set; }

        [Required(ErrorMessage = "Date là bắt buộc")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "StartTime là bắt buộc")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime là bắt buộc")]
        public DateTime EndTime { get; set; }

        public bool IsHoliday { get; set; }

        [Required(ErrorMessage = "CashFloat là bắt buộc")]
        public decimal CashFloat { get; set; }

        [Required(ErrorMessage = "Total là bắt buộc")]
        public decimal Total { get; set; }
    }
}
