using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum Status
    {
        [Display(Name = "Active")]
        Active = 0,
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Disabled")]
        Disabled,
    }
    //public static class Status
    //{
    //    public const int Active = 0;
    //    public const int Pending = 1;
    //}
}
