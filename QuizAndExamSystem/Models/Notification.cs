
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Notification : BaseEntity
    {
        [Display(Name = "Issue Date:")]
        public DateTime NotificationDate { get; set; }
        [Display(Name = "Issued By:")]
        public string? IssuedBy { get; set; }
        public string? NotificationFile { get; set; }



    }
}
