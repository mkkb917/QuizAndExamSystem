using ExamSystem.Models;
using System.ComponentModel.DataAnnotations;


namespace ExamSystem.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Name is required")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Name is required")]
        public string? LastName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string?  Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string?  Password { get; set; }

        [Display(Name="Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password do not match")]
        public string?  ConfirmPassword { get; set; }


        [Display(Name = "Account Type")]
        public string?  Role { get; set; }
    }
}
