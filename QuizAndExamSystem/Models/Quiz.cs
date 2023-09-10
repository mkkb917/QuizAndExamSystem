using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Models
{
    public class Quiz : BaseEntity
    {
        [Display(Name = "Quiz Name")]
        public string? QuizName { get; set; }
        [Display(Name = "Class")]
        public string? Grade { get; set; }
        [Display(Name = "Subject")]
        public string? Subject { get; set; }
        [Display(Name = "Total Marks")]
        public int TotalScores { get; set; }
        [Display(Name = "Obtained Marks")]
        public int ObtainedScores { get; set; }
        [Display(Name = "Result")]
        public string? Result { get; set; }
    }
}
