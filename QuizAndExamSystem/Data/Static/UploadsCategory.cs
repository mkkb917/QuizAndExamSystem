using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum UploadsCategory
    {
        [Display(Name = "Syllabus")]
        Syllabus = 1,
        [Display(Name = "Past Papers")]
        PastPapers,
        [Display(Name = "Notes")]
        Notes,
        [Display(Name = "School Decorations")]
        Decorate,
        [Display(Name = "School Events")]
        Events,
        [Display(Name = "School Management")]
        Manage,
        [Display(Name = "Educational Calender")]
        Calender,
    }

    // for school decoration the following kinds are used as Code 

    public enum SyllabusEnum
    {
        [Display(Name = "9th class")]
        Tenth = 1,
        [Display(Name = "10th class")]
        Ninth,
    }
    public enum PastPapersEnum
    {
        [Display(Name = "10th")]
        Tenth = 1,
        [Display(Name = "9th")]
        Ninth,
    }
    public enum NotesEnum
    {
        [Display(Name = "10th")]
        Tenth = 1,
        [Display(Name = "9th")]
        Ninth,
    }
    public enum DecorateEnum
    {
        [Display(Name = "Panaflex")]
        Panaflex = 1,
        [Display(Name = "Charts")]
        Charts,
        [Display(Name = "Glass Images")]
        Glass_Cover,
        
    }
    public enum EventsEnum
    {
        [Display(Name = "Naats")]
        Naats = 1,
        [Display(Name = "Comparings")]
        Comparing,
        [Display(Name = "English Speaches")]
        Speach_English,
        [Display(Name = "Urdu Speaches")]
        Speach_Urdu,
    }
    public enum ManageEnum
    {
        [Display(Name = "Farog-e-Taleem Fund")]
        FTF = 1,
        [Display(Name = "Non-Salary Budget")]
        NSB,
        [Display(Name = "School Management Committee")]
        SMC,
        
    }
    public enum CalenderEnum
    {
        [Display(Name = "10th")]
        Tenth = 1,
        [Display(Name = "9th")]
        Ninth,

    }
}
