using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Question : BaseEntity
    {


        [Display(Name = "Question Title")]
        public string? QuestionText { get; set; } = string.Empty;

        [Display(Name = "Question in Urdu")]
        public string? QuestionTextL { get; set; } = string.Empty;
        public QuestionTypes QuestionType { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }


        //relationships (1 to 1 for choice and 1 to many for meta)
        //public List<Choice> Choice { get; set; } 
        public virtual Choice Choice { get; set; }
        public List<QuestionMeta>? QuestionMeta { get; set; }

        //navigation back to Topics
        public int TopicId { get; set; }
        public Topic? Topic { get; set; }


    }
}


