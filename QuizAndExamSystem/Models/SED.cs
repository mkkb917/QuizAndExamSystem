using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class SED : BaseEntity
    {
        public string? Title { get; set; }
        [Display(Name = "Issue Date:")]
        public DateTime Date { get; set; }
        [Display(Name = "Issued By:")]
        public string? IssuedBy { get; set; }
        public string? FileName { get; set; }
        public string? FileFaceImage { get; set; }

    }
}
