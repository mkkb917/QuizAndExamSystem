using Entities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class QnAs
    {

        public int QuestionID { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionTextL { get; set; }
        public QuestionTypes QuestionType { get; set; }
        //public DifficultyLevel QuestionDifficulty { get; set; }
        public Choice? OptionsQnA { get; set; }
        public int? SelectedAnswer { get; set; }
        public int AnswerId { get; set;}


    }
    public class OptionsDetails     // not being used 
    {
        public int OptionID { get; set; }
        public string? Option1 { get; set; }
        public string? OptionL1 { get; set; }
        public string? Option2 { get; set; }
        public string? OptionL2 { get; set; }
        public string? Option3 { get; set; }
        public string? OptionL3 { get; set; }
        public string? Option4 { get; set; }
        public string? OptionL4 { get; set; }
        public string? Answer { get; set; }
        public string? AnswerL { get; set; }
    }

    public class QuizOptions
    {
        public string? UserName { get; set; }
        public string? QuizName { get; set; }
        public string? Grade { get; set; }
        public string? Subject { get; set; }
        public string? Duration { get; set; }
        public int TotalMarks { get; set; }


        public List<int>? QuestionID { get; set;}
        public List<int>? AnswerId { get; set; }
        public List<int>? SelectedAnswer { get; set; }

    }

}
    

