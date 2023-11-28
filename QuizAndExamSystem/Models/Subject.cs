using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Subject : BaseEntity
    {
        [Required(ErrorMessage = "Subject Name is required")]
        [Display(Name = "Subject Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "TextLength must be between 3 and 50 characters")]
        public string? SubjectText { get; set; }
        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Subject Image")]
        public string? Image { get; set; }

        //relationship to topics   one to many
        public List<Topic>? Topic { get; set; }

        // navigation back to classes
        public int GradeId { get; set; }
        public Grade? Grade { get; set; }

    }
}
