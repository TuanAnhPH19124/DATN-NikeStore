using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesDto
{
    public class AddressAPI
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public int CityCode { get; set; }

        [Required]
        public int ProvinceCode { get; set; }

        [Required]
        public string WardCode { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Số điện thoại phải đủ 10 số")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool SetAsDefault { get; set; }
    }

    public class AddressUpdateAPI : AddressAPI
    {
        [Required]
        public string Id { get; set; }
    }

}
