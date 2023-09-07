using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AppUserDto
    {

    }

    public class AppUserPhoneDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }

    public class AddressDto
    {
        public string Id { get; set; }
        public string AddressLine { get; set; }
        public int CityCode { get; set; }
        public int ProvinceCode { get; set; }
        public string WardCode { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool SetAsDefault { get; set; }
    }
}
