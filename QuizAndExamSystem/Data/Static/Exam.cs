using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum Exam
    {
        [Display(Name = "Annual")]
        Annual,
        [Display(Name = "Supplimentry")]
        Supplimentry
    }
}
