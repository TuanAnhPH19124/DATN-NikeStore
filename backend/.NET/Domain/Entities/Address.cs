using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Line { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Ward { get; set; }
        public int ProvinceId { get; set; }
        public int toDistrictId { get; set; }
        public string WardCode { get; set; }

        public string UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
