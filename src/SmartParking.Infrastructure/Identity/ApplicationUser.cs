using Microsoft.AspNetCore.Identity;
using SmartParking.Domain.Enums;
using SmartParking.Domain.Entities;

namespace SmartParking.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid? SelectedPricingPlanId { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;

        public PricingPlan? SelectedPricingPlan { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }
}