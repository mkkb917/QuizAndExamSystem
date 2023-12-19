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
        public string? Code { get; set; } = "NoCode";

        // question details
        //[Required(ErrorMessage ="Enter the Question here")]
        [Display(Name = "Quesion")]
        public string? QuestionText { get; set; }                   //
        //[Required(ErrorMessage = "Enter the Question in Urdu")]
        [Display(Name = "Quesion L")]
        public string? QuestionTextL { get; set; }                  //
        
        [Required(ErrorMessage ="Select the question Type")]
        [Display(Name = "Quesion Type")]
        public QuestionTypes  QuestionType { get; set; }                      //
        [Display(Name = "Quesion Status")]
        public Status? Status { get; set; }                   //
        [Display(Name = "Difficulty Level")]
        public DifficultyLevel DifficultyLevel { get; set; }        //
        //[Required(ErrorMessage = "Enter the Question description")]
        public string? Description { get; set; } = "Description";
        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;       //
        [Display(Name = "Created by")]
        public string? CreatedBy { get; set; } = string.Empty;        //
        [Display(Name = "Updated on")]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;         //
        [Display(Name = "Updated by")]
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
        [Display(Name = "Answer")]
        public int SelectedAnswer { get; set; }

        //metadata 
        public List<QuestionMetaVM>? QuestionMetaVMs { get; set; }
    }

    public class QuestionMetaVM
    {
        public int QuestionId { get; set; }
        //metadata 
        [Display(Name = "Board Name:")]
        public BoardNames BoardName { get; set; }                 //
        [Display(Name = "Examination Name:")]
        public Exam ExamName { get; set; }                   //
        [Display(Name = "Examination Year:")]
        public DateTime ExamYear { get; set; }                  //
        [Display(Name = "Session:")]
        public Session Session { get; set; }                  //
        [Display(Name = "Mark as")]
        public MarkAs MarkAs { get; set; }
        public string? Keywords { get; set; }                   //
        public string? Description { get; set; }
    }
    public class QuestionIndexVM
    {
        public List<Question>? Questions { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "Topic Name")]
        public string? TopicName { get; set; }
        public int SubjectId { get; set; }
        [Display(Name = "Subject Name")]
        public string? SubjectName { get; set; }
    }
}
