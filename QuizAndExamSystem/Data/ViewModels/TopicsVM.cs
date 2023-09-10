using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;
using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class TopicsVM
    {
        
        public int Id { get; set; }
        [Display(Name = "Unit Name")]
        [Required(ErrorMessage = "Unit Name is required")]
        public string? TopicText { get; set; }
        [Required(ErrorMessage ="Unit Code is required")]
        [Display(Name ="Unit Code")]
        public string? Code { get; set; }
        public Status? Status { get; set; }
        [Display(Name = "Description")]
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Updated by")]
        public string? UpdatedBy { get; set; }
        [Display(Name = "Updated on")]
        public DateTime UpdatedOn { get; set; }

        public int GradeId { get; set; }
        //[Display(Name = "Grade Name")]
        //public string Grade { get; set; }

        [Display(Name = "Subject")]
        public int SubjectId { get; set; }
        [Display(Name = "Subject Name")]
        public string? SubjectText { get; set; }
        [Display(Name = "User Name")]
        public string? user { get; set; }
        public List<Question>? Questions { get; set; }

        // properties for pairing scheme mapping.
        [Display(Name = "Total MCQs")]
        public int MCQCount { get; set; }
        [Display(Name = "MCQs Marks")]
        public int MCQMarks { get; set; }
        [Display(Name = "Total SEQs")]
        public int SEQCount { get; set; }
        [Display(Name = "SEQs Marks")]
        public int SEQMarks { get; set; }
        [Display(Name = "Total Long Qs")]
        public int LongQCount { get; set; }
        [Display(Name = "Long Qs Marks")]
        public int LongQMarks { get; set; }
    }


    public class TopicIndexVM
    {
        public List<Topic>? Topic { get; set; }
        public int GradeId { get; set; }
        public string? GradeName { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
    }
}
