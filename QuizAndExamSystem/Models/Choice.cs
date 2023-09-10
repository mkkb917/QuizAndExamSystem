using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    // removed inheritance from BaseEntity class due to reduce columns  createdon, createdby,updatedon and updatedby
    public class Choice
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Choice1 { get; set; }
        public string? ChoiceL1 { get; set; }
        public string? Choice2 { get; set; }
        public string? ChoiceL2 { get; set; }
        public string? Choice3 { get; set; }
        public string? ChoiceL3 { get; set; }
        public string? Choice4 { get; set; }
        public string? ChoiceL4 { get; set; }
        public string? Answer { get; set; }
        public string? AnswerL { get; set; }

        //relationship back to Question class
        public int QuestionId { get; set; }
        //public Question Question { get; set; }
        public virtual Question Question { get; set; }

    }
}
