using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;
using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class SubjectsIndexVM
    {
        public List<Subject>? SubjectVm { get; set; }
        public int GradeId { get; set; }
        public string? GradeName { get; set; }
        //public int TopicCount { get; set; }

    }
}
