﻿
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Grade : BaseEntity
    {

        [Required(ErrorMessage = "Grade Name is required")]
        [Display(Name = "Class Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "TextLength must be between 3 and 50 characters")]
        public string? GradeText { get; set; }
        //[Required(ErrorMessage = "Class Image is required")]
        [Display(Name = "Class Image")]
        public string? Image { get; set; } 



        //one to many relationship | one Grade has many Subjects
        public List<Subject>? Subject { get; set; }
    }
}
