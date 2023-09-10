using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum Gender
    {
       
        [Display(Name ="Male")]
        Male,
        [Display(Name = "Female")]
        Female,
        [Display(Name = "Transgender")]
        Transgender,
        [Display(Name = "Not to say")]
        unspecified
    }
    public enum SchoolType
    {
        
        [Display(Name = "Public")]
        Public,
        [Display(Name = "Private")]
        Private,
        [Display(Name = "Semi Government")]
        SemiGovernment,
    }
}
