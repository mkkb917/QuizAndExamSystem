namespace Entities.Models
{
    public class Marks:BaseEntity
    {
        public string? QuestionType { get; set; }
        public string? MarksAllocated { get; set; }
        public string? UserId { get; set; }
    }
}
