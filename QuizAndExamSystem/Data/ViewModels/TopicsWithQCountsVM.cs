using ExamSystem.Data.Static;

namespace ExamSystem.Data.ViewModels
{
    public class TopicsWithQCountsVM
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? TopicText { get; set; }
        public int McqCount { get; set; }
        public int SeqCount { get; set; }
        public int LongQCount { get; set; }
        public int FillinBlankCount { get; set; }
    }

}
 
