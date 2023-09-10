using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum Exam
    {
        [Display(Name = "Annual")]
        Annual,
        [Display(Name = "Supplimentry")]
        Supplimentry
    }
}
