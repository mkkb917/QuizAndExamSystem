using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class GeneratedPaper
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Class")]
        public string? Grade { get; set; }
        [Display(Name = "Subject")]
        public string? Subject { get; set; }
        [Display(Name = "Objective Paper")]
        public string? PaperFile { get; set; }
        [Display(Name = "Subjective Paper")]
        public string? PaperSubjetiveFile { get; set; }
        [Display(Name = "Solution")]
        public string? SolutionFile { get; set; }
        public string? Barcode { get; set; }
        public bool IsAstive { get; set; }
    }
}
