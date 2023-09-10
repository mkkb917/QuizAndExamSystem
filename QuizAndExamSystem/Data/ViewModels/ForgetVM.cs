using System.ComponentModel.DataAnnotations;


namespace ExamSystem.Data.ViewModels
{
    public class ForgetVM
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
