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
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Status Status { get; set; } = Status.Active;
        public DateTime ModifiedDate { get; set; }
        public Ranks Rank { get; set; } = Ranks.New;
        public decimal LoyaltyPoint { get; set; } = 0;
        public int MemberDiscount { get; set; } = 0;
        public string AvatarUrl { get; set; }
    }
}
