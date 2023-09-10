using ExamSystem.Data.Base;
using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class BaseEntity : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public Status? Status { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Plz Enter the code")]
        public string? Code { get; set; }
        [Display(Name = "Description")]
        public string? Description { get; set; } = string.Empty;
        [Display(Name = "Created by")]
        public string? CreatedBy { get; set; } = string.Empty;
        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Updated by")]
        public string? UpdatedBy { get; set; } = string.Empty;
        [Display(Name = "Updated on")]
        public DateTime UpdatedOn { get; set; }
    }
}
