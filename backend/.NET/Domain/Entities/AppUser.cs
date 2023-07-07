using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        
        public string FullName { get; set; }
        public Status Status { get; set; } = Status.Active;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public Ranks Rank { get; set; } = Ranks.New;
        public decimal LoyaltyPoint { get; set; } = 0;
        public int MemberDiscount { get; set; } = 0;
        public string AvatarUrl { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
