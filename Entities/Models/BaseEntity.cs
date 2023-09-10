using Entities.Static;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class BaseEntity/*:IEntityBase*/
    {
        [Key]
        public int Id { get; set; }
        
        public Status? Status { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage ="Plz Enter the code")]
        public string? Code { get; set; }
        [Display(Name = "Description")]
        public string? Description { get; set; } = String.Empty;
        [Display(Name = "Created by")]
        public string? CreatedBy { get; set; } = String.Empty;
        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Updated by")]
        public string? UpdatedBy { get; set; } = String.Empty;
        [Display(Name = "Updated on")]
        public DateTime UpdatedOn { get; set; }
    }
}
