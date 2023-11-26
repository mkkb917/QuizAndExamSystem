using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;
using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class QuestionVM
    {
        public int Id { get; set; }
        
        
        // set up category
        [Required(ErrorMessage = "Select the Class for this Question")]
        public int GradeId { get; set; }
        [Required(ErrorMessage = "Select the Subject for this Question")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Select the Unit for this Question")]
        public int TopicId { get; set; }

        // question details
        public string? Code { get; set; } = "NoCode";
        [Required(ErrorMessage ="Enter the Question here")]
        public string? QuestionText { get; set; }                   //
        [Required(ErrorMessage = "Enter the Question in Urdu")]
        public string? QuestionTextL { get; set; }                  //
        
        [Required(ErrorMessage ="Select the question Type")]
        public QuestionTypes  QuestionType { get; set; }                      //
        [Required(ErrorMessage = "Select the question Difficulty level")]
        public Status? Status { get; set; }                   //
        public DifficultyLevel DifficultyLevel { get; set; }        //
        //[Required(ErrorMessage = "Enter the Question description")]
        public string? Description { get; set; } = "description";                  //
        public DateTime CreatedOn { get; set; } = DateTime.Now;       //
        public string? CreatedBy { get; set; } = string.Empty;        //
        public DateTime UpdatedOn { get; set; } = DateTime.Now;         //
        public string? UpdatedBy { get; set; } = string.Empty;  //

        // Choices 
        //public int ChoiceId { get; set; }  
        
        public string? ChoiceTitle1 { get; set; }               //
        public string? ChoiceTitleL1 { get; set; }              //
        public string? ChoiceTitle2 { get; set; }               //
        public string? ChoiceTitleL2 { get; set; }              //
        public string? ChoiceTitle3 { get; set; }               //
        public string? ChoiceTitleL3 { get; set; }              //
        public string? ChoiceTitle4 { get; set; }               //
        public string? ChoiceTitleL4 { get; set; }              //  
        public string? CorrectAnswer { get; set; }              //
        public string? CorrectAnswerL { get; set; }             //

        public int SelectedAnswer { get; set; }

        //metadata 
        [Display(Name = "Board Name:")]
        public BoardNames BoardName { get; set; }                 //
        [Display(Name = "Examination Name:")]
        public Exam ExamName { get; set; }                   //
        [Display(Name = "Examination Year:")]
        public DateTime ExamYear { get; set; }                  //
        [Display(Name = "Session:")]
        public Session SessionId { get; set; }                  //
        public MarkAs MarkAs { get; set; }
        public string? Keywords { get; set; }                   //
    }

    public class QuestionIndexVM
    {
        public List<Question>? Questions { get; set; }
        public int TopicId { get; set; }
        public string? TopicName { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
    }
}
