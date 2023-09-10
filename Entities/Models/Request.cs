using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Option
    {
        public string? User { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public int TotalMarks { get; set; }
        public string? Duration { get; set; }
        public int QuizID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public int SelectedOption { get; set; }
    }
    public class Root
    {
        public ApplicationUser? objCandidate { get; set; }
        public List<QuizAttempt>? objAttempt { get; set; }
    }
    public class QuizAttempt
    {
        public int SlNo { get; set; }
        public int QuizID { get; set; }
        public string? Quiz { get; set; }
        public DateTime Date { get; set; }
        public string? Score { get; set; }
        public string? Status { get; set; }
    }
    public class QuizReport
    {
        public int UserId { get; set; }
        public int QuizID { get; set; }
        public string? Quiz { get; set; }
        public string? Date { get; set; }
        public string? Message { get; set; }
    }
    public class ReqReport
    {
        public int QuizID { get; set; }
        public string? UserId { get; set; }
        public string? SessionID { get; set; }
    }
    public class ReqCertificate
    {
        public int UserId { get; set; }
        public int QuizID { get; set; }
        public string? Exam { get; set; }
        public string? Date { get; set; }
        public string? Score { get; set; }
    }

    public class Request
    {
    }
    public class ResPDF
    {
        public bool IsSuccess { get; set; }
        public string? Path { get; set; }
    }
}
