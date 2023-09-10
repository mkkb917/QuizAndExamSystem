using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum SEDCategory
    {
        [Display(Name = "Notification")]
        Notifications,
        [Display(Name = "Orders")]
        Orders,
        [Display(Name = "Judgements")]
        Judgements,
        [Display(Name = "Policy")]
        Policy,
    }
}
