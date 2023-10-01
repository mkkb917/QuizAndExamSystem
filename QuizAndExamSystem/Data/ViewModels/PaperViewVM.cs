using ExamSystem.Data.Static;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class PaperViewVM
    {
        // papersetting metadata details
        public PaperSetting? Setting { get; set; }
        public List<QnAs>? MCQs { get; set; }
        public List<QnAs>? SEQs { get; set; }
        public List<QnAs>? LongQs { get; set; }


    }
}
