using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {

        public string FullName { get; set; }
        public Status Status { get; set; } = Status.ACTIVE;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public Ranks Rank { get; set; } = Ranks.New;
        public decimal LoyaltyPoint { get; set; } = 0;
        public int MemberDiscount { get; set; } = 0;
        public string AvatarUrl { get; set; }


        public Employee Employee { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual IEnumerable<ShoppingCarts> ShoppingCarts { get; set; }
        public virtual ICollection<ProductRate> ProductRate { get; set; }
    }
}
