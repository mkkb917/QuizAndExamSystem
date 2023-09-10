using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum Status
    {
        [Display(Name = "Active")]
        Active = 0,
        [Display(Name = "Pending")]
        Inactive,
    }
    //public static class Status
    //{
    //    public const int Active = 0;
    //    public const int Inactive = 1;
    //}
}
