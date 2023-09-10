using ExamSystem.Data.Static;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class QuizViewVM
    {
        public QuizViewVM() {
            QuizMcqs = new List<QnAs>();
        }
        public string? UserName { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public int TotalMarks { get; set; }
        public string? Duration { get; set; }
        public List<QnAs>? QuizMcqs { get; set; }
    }
    public class QuizData:QnAs
    {
        public int QuestionId { get; set; }
        public new int AnswerId { get; set; }
        public int SelectedOption { get; set; }
    }

    
    
}
