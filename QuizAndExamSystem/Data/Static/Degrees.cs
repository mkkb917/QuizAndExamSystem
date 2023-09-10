using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum Degrees
    {
       
        [Display(Name = "Matric")]
        Matriculation,
        [Display(Name = "Inter")]
        Intermediate,
        [Display(Name = "Bachlors")]
        Bachelor,
        [Display(Name = "Masters")]
        Master,
        [Display(Name = "M.Phil")]
        M_Phil,
        [Display(Name = "PhD")]
        Ph_D,
    }
}
