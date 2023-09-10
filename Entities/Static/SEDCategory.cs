using System.ComponentModel.DataAnnotations;

namespace Entities.Static
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
