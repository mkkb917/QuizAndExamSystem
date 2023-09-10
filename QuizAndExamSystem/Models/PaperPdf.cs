namespace ExamSystem.Models
{
    public class PaperPdf : BaseEntity
    {
        public string? PaperName { get; set; }
        public ApplicationUser? UserId { get; set; }
        public int SubjectId { get; set; }
        public int QuestionID { get; set; }
        public int ChoiceId { get; set; }
    }
}
