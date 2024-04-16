using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; } // Subscription ID
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = false; // Status (active/inactive)
        public string? BillingInfo { get; set; } = string.Empty; // Billing details
        public DateTime RenewalDate { get; set; } // Renewal date (if applicable)

        //navigational properties
        //one to many relationships
        // Shadow property   because of string userID cannt relate ti int subscriptionID
        private string _UserId;
        public string UserId
        {
            get => _UserId ?? User?.Id;
            set => _UserId = value;
        }
        //public string UserId { get; set; }  // Add ApplicationUserId as part of the composite key
        public virtual ApplicationUser? User { get; set; }

        public int PlanId { get; set; } // Plan ID (foreign key)
        public virtual List<Plan>? Plan { get; set; }
    }
}
