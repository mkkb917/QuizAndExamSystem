using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string? Email { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; } 

        [Display(Name = "Remember me")]
        public Boolean RememberMe { get; set; }

        // return url property is created for external logins like google and facebook etc
        public string? ReturnUrl { get; set; } 

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
