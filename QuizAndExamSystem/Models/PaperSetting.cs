using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class PaperSetting : BaseEntity
    {
        // user info
        public string? UserName { get; set; }
        // school info
        [Display(Name = "Logo")]
        public string? SchoolLogo { get; set; }
        [Display(Name = "School Name")]
        public string? SchoolName { get; set; }
        // exam metadata
        [Display(Name = "Examination")]
        public string? ExamName { get; set; }
        [Display(Name = "Class Name")]
        public string? ClassName { get; set; }
        [Display(Name = "Teacher Name")]
        public string? TeacherName { get; set; }
        [Display(Name = "Subject Name")]
        public string? SubjectName { get; set; }
        [Display(Name = "Conduct Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ConductDate { get; set; }
        [Display(Name = "Duration in Minutes")]
        public string? Duration { get; set; }
        [Display(Name = "Difficulty Level")]
        public DifficultyLevel DifficultyLevel { get; set; }
        [Display(Name = "Medium ")]
        public Medium Medium { get; set; }
        [Display(Name = "Paper Instructions")]
        public string? Instructions { get; set; }
        // marks distribution
        [Display(Name = "Total Marks")]
        public float? TotalMarks { get; set; }
        [Display(Name = "Passing Marks")]
        public int PassingMarks { get; set; }
        [Display(Name = "Multiple Choice Question")]
        public float? MCQsMarks { get; set; }
        [Display(Name = "Multiple Choice Question Count")]
        public int MCQsCount { get; set; }
        [Display(Name = "Short Question")]
        public float? SEQsMarks { get; set; }
        [Display(Name = "Short Question Count")]
        public int? SEQsCount { get; set; }
        [Display(Name = "Long Question")]
        public float? LongQsMarks { get; set; }
        [Display(Name = "Long Question Count")]
        public int LongQsCount { get; set; }
        [Display(Name = "Fill in the Blanks")]
        public float? FillInBlanksMarks { get; set; }
        [Display(Name = "Fill in the Blanks Count")]
        public int FillInBlanksCount { get; set; }


        //public int PaperLayout { get; set; }



    }
}
