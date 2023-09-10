using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class QuestionMeta
    {
        [Key]
        public int Id { get; set; }
        public BoardNames BoardName { get; set; }
        public Exam ExamName { get; set; }
        public DateTime ExamYear { get; set; }
        public Session Session { get; set; }
        public string? Keywords { get; set; }
        public string? Description { get; set; }
        public MarkAs MarkAs { get; set; }

        //navigation back to question class
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
