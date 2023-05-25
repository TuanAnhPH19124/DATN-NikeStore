using Domain.Enums;
using System;


namespace Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string? AvatarUrl { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string? FullName { get; set; }
        public string PhoneNumber { get; set; }

        private Status _status = Enums.Status.Active;
        public Status Status { get => _status; set => _status = value; }

        public DateTime? ModifiedDate { get; set; }
        public string? Rank { get; set; }
        public decimal? LoyaltyPoint { get; set; } = 0;
        public int? MemberDiscount { get; set; } = 0;

    }
}
