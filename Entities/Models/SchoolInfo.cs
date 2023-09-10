using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class SchoolInfo
    {
        [Key]
        public string? Id { get; set; }
        //School Information
        [Required]
        [Display(Name = "School type")]
        public string? SchoolType { get; set; } = String.Empty; // public or private
        [Display(Name = "School Full Name")]
        public string? SchoolName { get; set; } = String.Empty;
        [Display(Name = "School EMIS Code")]
        public string? SchoolCode { get; set; } = String.Empty;
        [Display(Name = "School Email Address")]
        public string? SchoolEmail { get; set; } = String.Empty;
        [Display(Name = "School Contact Number")]
        public string? SchoolContactNumber { get; set; } = String.Empty;
        [Display(Name = "School Logo")]
        public string? SchoolLogo { get; set; } = String.Empty;
        [Display(Name = "School short description")]
        public string? SchoolDescription { get; set; } = String.Empty;
        [Display(Name = "School complete postal address")]
        public string? SchoolAddress { get; set; } = String.Empty;


        // one to one relation with ApplicationUser class
        public virtual ApplicationUser? AppUser { get; set; }
    }
}
