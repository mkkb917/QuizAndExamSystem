using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum QuestionTypes
    {
        
        [Display(Name="Multiple Choice Question")]
        MCQ,
        [Display(Name = "Short Examination Question")]
        SEQ,
        [Display(Name = "Long Examination Question")]
        Long_Question,
        [Display(Name = "Fill in the Blank")]
        Fill_In_The_Blanks
    }
}
