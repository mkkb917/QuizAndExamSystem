using ExamSystem.Data.Static;
using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.ViewModels
{
    public class SubjectsVM
    {
        public int Id { get; set; }

        //public string? SubjectCode { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Please Enter the Subejct Name")]
        public string? SubjectText { get; set; }
        [Display(Name = "Subject Code")]
        [Required(ErrorMessage ="Please Enter the Subejct Code")]
        public string? Code { get; set; } = "NoCode";
        [Display(Name = "Subject Image")]
        //[Required(ErrorMessage = "Please Enter the Subejct Code")]
        public string? Image { get; set; } = string.Empty;
        public List<Topic>? Topics { get; set; }
        public Status? Status { get; set; }
        [Display(Name = "Subject Description")]
        [Required(ErrorMessage = "Please Enter the Subejct Description")]
        public string? Description { get; set; }
        public int GradeId { get; set; }
        [Display(Name = "Grade Name")]
        public string? Grade { get; set; }
        [Display(Name = "Created by")]
        public string? CreatedBy { get; set; }
        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Updated by")]
        public string? UpdatedBy { get; set; }
        [Display(Name = "Updated on")]
        public DateTime UpdatedOn { get; set; }   
        public string? user { get; set; }

        
    }
}
