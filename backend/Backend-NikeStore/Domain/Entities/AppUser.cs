using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        private Status _status = Enums.Status.Active;
        public Status Status { get => _status; set => _status = value; }
        public DateTime ModifiedDate { get; set; }
        private Ranks _rank = Ranks.New;
        public Ranks MyProperty { get => _rank; set => _rank = value; }
        public decimal LoyaltyPoint { get; set; } = 0;
        public int MemberDiscount { get; set; } = 0;
        public string? AvatarUrl { get; set; }
    }
}
