using ExamSystem.Data.Static;
using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.ViewModels
{
    public class SchoolVM
    {
        public string?  Id { get; set; }

        //school information
        [Required]
        [Display(Name = "School type")]
        public string? SchoolType { get; set; }  // public or private
        [Display(Name = "School Name")]
        public string? SchoolName { get; set; }
        [Display(Name = "School EMIS Code")]
        public string? SchoolCode { get; set; } 
        [Display(Name = "School Email Address")]
        public string? SchoolEmail { get; set; }
        [Display(Name = "School Contact Number")]
        public string? SchoolContactNumber { get; set; }
        [Display(Name = "School Logo")]
        public string? SchoolLogo { get; set; }
        [Display(Name = "School short description")]
        public string? SchoolDescription { get; set; }
        [Display(Name = "School address")]
        public string? SchoolAddress { get; set; }
    }
}
