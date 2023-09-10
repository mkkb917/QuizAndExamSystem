using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum UploadsCategory
    {
        [Display(Name = "Syllabus")]
        Syllabus =1,
        [Display(Name = "Past Papers")]
        PastPapers,
        [Display(Name = "Notes")]
        Notes,
        [Display(Name = "Books")]
        Books,
        
    }
}
