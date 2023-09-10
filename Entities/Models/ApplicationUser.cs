using Entities.Static;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.Models
{
    public class ApplicationUser:IdentityUser
    {

        public override string? Id { get; set; }

        [Display(Name="First Name:")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string?  FirstName { get; set; } = String.Empty;


        [Display(Name = "Last Name:")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string? LastName { get; set; } = String.Empty;

        public Gender Gender { get; set; }

        [Display(Name = "User Name:")]
        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "User Name must be between 5 to 30 characters")]
        public override string?  UserName { get; set; } =  string.Empty;
        // personal information
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        [Display(Name = "Biography")]
        [DataType(DataType.MultilineText)]
        public string?   Biography { get; set; } = string.Empty;
        [Display(Name = "Father Name:")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string?   FatherName { get; set; } = String.Empty;

        [Display(Name = "CNIC Number:")]
        [DisplayFormat(DataFormatString = "{0:#####-#######-#}")]
        [Required(ErrorMessage = "National ID is a required field")]
        [DataType(DataType.Text)]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "National ID must be 13 digits")]
        //[RegularExpression(@"^\( ([0-9]{14})", ErrorMessage = "Not a valid ID Number")]
        public string?   CNIC { get; set; } = String.Empty;
        [Display(Name = "Profile Image")]
        [DataType(DataType.ImageUrl)]
        public string?   ProfileImage { get; set; } = "/images/avatar/avatar.jpg"; 
        [Display(Name = "Contact Number:")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\( ([0-9]{4})\) [-. ] ([0-9]{3})[-. ] ([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string?   ContactNo { get; set; } = String.Empty;
        [Display(Name = "Cell Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\( ([0-9]{4})\) [-. ] ([0-9]{3})[-. ] ([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string?   CellPhoneNo { get; set; } = String.Empty;
        [Display(Name = "Whatsapp Number:")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\( ([0-9]{4})\) [-. ] ([0-9]{3})[-. ] ([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string?   WhatsApp { get; set; } = String.Empty;

        //Residential Information
        [Display(Name = "Home Address:")]
        [DataType(DataType.Text)]
        public string?   HomeAddress { get; set; } = String.Empty;
        [Display(Name = "City:")]
        [DataType(DataType.MultilineText)]
        public string?   City { get; set; } = String.Empty;
        [Display(Name = "Postal Code:")]
        [DataType(DataType.PostalCode)]
        public string?   PostalCode { get; set; } = String.Empty;
        [Display(Name = "State:")]
        [DataType(DataType.Text)]
        public string?   State { get; set; } = String.Empty;
        [Display(Name = "Country:")]
        [DataType(DataType.Text)]
        public string?   Country { get; set; } = String.Empty;

        //Qualification information
        [Display(Name = "Study Level")]
        public Degrees HighestDegree { get; set; }  //1=> matric, 2=> FA, 3=>B.A, 4=> MA, 5=>mPhil, 6=>Phd
        [Display(Name = "Degree name")]
        public string?   DegreeName { get; set; } = String.Empty;

        //Social Media Details
        [Display(Name = "Facebook Profile Link")]
        public string?   Facebook { get; set; } = String.Empty;
        [Display(Name = "Twitter Profile Link")]
        public string?   Twitter { get; set; } = String.Empty;
        [Display(Name = "Linkedin Profile Link")]
        public string?   Linkedin { get; set; } = String.Empty;
        [Display(Name = "Instagram Profile Link")]
        public string?   Instagram { get; set; } = String.Empty;
        [Display(Name = "Pinterest Profile Link")]
        public string?   Pinterest { get; set; } = String.Empty;
        
        // relationship  to school info ( 1 to 1)
        [ForeignKey("Id")]
        public virtual SchoolInfo  SchoolInfo { get; set; }

        
    }
}
