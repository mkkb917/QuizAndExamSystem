using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum BookCategory
    {
        [Display(Name = "AIOU")]
        AIOU,
        [Display(Name = "Syllabus")]
        Syllabus,
        [Display(Name = "Stories")]
        Stories,
        [Display(Name = "Islamic")]
        Islamic,
        [Display(Name ="General Knowledge")]
        General_Knowledge,
        [Display(Name = "Public Service Commission")]
        PPSC,
    }
}
