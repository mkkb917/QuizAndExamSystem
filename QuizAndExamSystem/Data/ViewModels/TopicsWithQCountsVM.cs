using ExamSystem.Data.Static;

namespace ExamSystem.Data.ViewModels
{
    public class TopicsWithQCountsVM
    {
        
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? TopicText { get; set; }
        public int McqMarks { get; set; }
        public int McqCount { get; set; }
        public int SeqMarks { get; set; }
        public int SeqCount { get; set; }
        public int LongQMarks { get; set; }
        public int LongQCount { get; set; }
        public int FillinBlankMarks { get; set; }
        public int FillinBlankCount { get; set; }
    }

    //VM Class for generate paper postback to controller  
    public class CheckboxItemForTopics
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
    }

}



