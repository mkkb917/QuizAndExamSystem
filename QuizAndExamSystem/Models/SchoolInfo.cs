using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class SchoolInfo
    {
        [Key]
        public string? Id { get; set; }
        //School Information
        [Required]
        [Display(Name = "School type")]
        public string? SchoolType { get; set; } = string.Empty; // public or private
        [Display(Name = "School Full Name")]
        public string? SchoolName { get; set; } = string.Empty;
        [Display(Name = "School EMIS Code")]
        public string? SchoolCode { get; set; } = string.Empty;
        [Display(Name = "School Email Address")]
        public string? SchoolEmail { get; set; } = string.Empty;
        [Display(Name = "School Contact Number")]
        public string? SchoolContactNumber { get; set; } = string.Empty;
        [Display(Name = "School Logo")]
        public string? SchoolLogo { get; set; } = string.Empty;
        [Display(Name = "School short description")]
        public string? SchoolDescription { get; set; } = string.Empty;
        [Display(Name = "School complete postal address")]
        public string? SchoolAddress { get; set; } = string.Empty;
        //public string? SchoolMedium { get; set; } = string.Empty;


        // one to one relation with ApplicationUser class
        public virtual ApplicationUser? AppUser { get; set; }
    }
}
