using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Topic : BaseEntity
    {

        [Display(Name = "Unit Name")]

        public string? TopicText { get; set; }

        //properties for pairing scheme.
        public int MCQCount { get; set; }
        public int MCQMarks { get; set; }
        public int SEQCount { get; set; }
        public int SEQMarks { get; set; }
        public int LongQCount { get; set; }
        public int LongQMarks { get; set; }

        //relationship ( one to many)
        public List<Question>? Question { get; set; }

        //navigation back to  subjects
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}
