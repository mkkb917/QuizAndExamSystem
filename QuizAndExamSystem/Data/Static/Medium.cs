using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum Medium
    {
        [Display(Name = "Both English and Urdu")]
        Default,
        [Display(Name = "English")]
        English,
        [Display(Name = "Urdu")]
        Urdu,
    }
}
