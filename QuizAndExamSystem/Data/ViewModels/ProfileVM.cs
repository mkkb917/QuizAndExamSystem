using ExamSystem.Data.Static;
using ExamSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.ViewModels
{
    public class ProfileVM
    {
        //account information
        public string? Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public List<string> Permissions { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public Status Status { get; set; } 
        //personal information
        [Display(Name = "First Name:")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? FirstName { get; set; } = String.Empty;
        [Display(Name = "Last Name:")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string? LastName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Please Enter Your Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please Enter Your User Name")]
        [Display(Name = "User Name:")]
        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "User Name must be between 5 to 30 characters")]
        public string? UserName { get; set; } = string.Empty;

        // personal information
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd, MM, yyyy}")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Biography")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please Enter Your Biography")]
        public string? Biography { get; set; } = string.Empty;
        [Display(Name = "Gender")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Your Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "Father Name:")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Required(ErrorMessage = "Please Enter Your Father Name")]
        public string? FatherName { get; set; } = String.Empty;
        [Display(Name = "CNIC Number:")]
        [DisplayFormat(DataFormatString = "{0:#####-#######-#}")]
        [Required(ErrorMessage = "Please Enter Your CNIC Number")]
        public string? CNIC { get; set; } = String.Empty;
        [Display(Name = "Profile Image")]
        [DataType(DataType.ImageUrl)]
        public string? ProfileImage { get; set; } = String.Empty;
        [Display(Name = "Contact Number:")]
        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\( ([0-9]{4})\) [-. ] ([0-9]{3})[-. ] ([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? ContactNo { get; set; } = String.Empty;
        [Display(Name = "Cell Phone Number")]
        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\( ([0-9]{4})\) [-. ] ([0-9]{3})[-. ] ([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? CellPhoneNo { get; set; } = String.Empty;
        [Required(ErrorMessage = "Please Enter Your Whatsapp Number")]
        [Display(Name = "Whatsapp Number:")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\( ([0-9]{4})\) [-. ] ([0-9]{3})[-. ] ([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? WhatsApp { get; set; } = String.Empty;

        //Residential Information
        [Display(Name = "Home Address:")]
        [Required(ErrorMessage = "Please Enter Your Home Address")]
        [DataType(DataType.Text)]
        public string? HomeAddress { get; set; } = String.Empty;
        [Required(ErrorMessage = "Please Enter Your City Name")]
        [Display(Name = "City:")]
        [DataType(DataType.Text)]
        public string? City { get; set; } = String.Empty;
        [Required(ErrorMessage = "Please Enter Your City Postal Code")]
        [Display(Name = "Postal Code:")]
        [DataType(DataType.PostalCode)]
        public string? PostalCode { get; set; } = String.Empty;
        [Required(ErrorMessage = "Please Enter Your Province or State Name")]
        [Display(Name = "State:")]
        [DataType(DataType.Text)]
        public string? State { get; set; } = String.Empty;
        [Required(ErrorMessage = "Please Enter Your Country Name")]
        [Display(Name = "Country:")]
        [DataType(DataType.Text)]
        public string? Country { get; set; } = String.Empty;

        //Qualification information
        [Display(Name = "Study Level")]
        public Degrees HighestDegree { get; set; }  //1=> matric, 2=> FA, 3=>B.A, 4=> MA, 5=>mPhil, 6=>Phd
        [Display(Name = "Degree name")]
        [Required(ErrorMessage = "Please Enter Your Higest Degree Name")]
        public string? DegreeName { get; set; } = String.Empty;

        //Social Media Details
        [Display(Name = "Facebook Profile Link")]
        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Please paste Your Facebook profile link ")]
        public string? Facebook { get; set; } = "https://www.facebook.com/";

        [Display(Name = "Twitter Profile Link")]
        [Required(ErrorMessage = "Please paste Your Twitter profile link ")]
        [BindProperty(SupportsGet = true)]
        public string? Twitter { get; set; } = "https://twitter.com/";

        [Display(Name = "Linkedin Profile Link")]
        [Required(ErrorMessage = "Please paste Your Linkedin profile link ")]
        [BindProperty(SupportsGet = true)]
        public string? Linkedin { get; set; } = "https://linkedin.com/";

        [Display(Name = "Instagram Profile Link")]
        [Required(ErrorMessage = "Please paste Your Instagram profile link ")]
        [BindProperty(SupportsGet = true)]
        public string? Instagram { get; set; } = "https://instagram.com/";

        [Display(Name = "Pinterest Profile Link")]
        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Please paste Your Pinterest profile link ")]
        public string? Pinterest { get; set; } = "https://pinterest.com/";



        // for only edit profile 

        // Property to populate the dropdown
        public List<SelectListItem> AvailablePermissions { get; set; }

        public ProfileVM()
        {
            AvailablePermissions = new List<SelectListItem>
            {
                new SelectListItem { Value = UserPermissions.Manager, Text = "Manager" },
                new SelectListItem { Value = UserPermissions.Moderator, Text = "Moderator" },
                new SelectListItem { Value = UserPermissions.Reader, Text = "Reader" }
                // Add other permissions as needed
            };
        }
    }
}
