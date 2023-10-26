using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum BookCategory
    {
        [Display(Name = "AIOU")]
        AIOU,
        [Display(Name = "Linguistics")]
        Linguistics,
        [Display(Name = "Stories")]
        Stories,
        [Display(Name = "Islamic")]
        Religious,
        [Display(Name ="General Knowledge")]
        General_Knowledge,
        [Display(Name = "Public Service Commission")]
        Public_Service,
    }
}
