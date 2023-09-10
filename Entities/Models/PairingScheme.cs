using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class PairingScheme : BaseEntity
    {
        public int SubjectId { get; set; }
        public string? GradeText { get; set; }
        public string? SubjectText { get; set; }
        public  string? TopicText { get; set; }
        public int MCQsCount { get; set; }
        public int MCQMarks { get; set; }
        public int SEQsCount { get; set; }
        public int SEQMarks { get; set; }
        public int LongQsCount { get; set; }
        public int LongQMarks { get; set; }
    }
}
