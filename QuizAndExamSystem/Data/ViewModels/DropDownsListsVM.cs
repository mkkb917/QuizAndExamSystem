using ExamSystem.Data.Static;
using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{


    public class DropDownsListsVM
    {
        public DropDownsListsVM()
        {
            Grades = new List<Grade>();
            Subjects = new List<Subject>();
            Topics = new List<Topic>();
            Questions = new List<Question>();

        }
        public List<Grade> Grades { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Question> Questions { get; set; }

        public int GradeId { get; set; }
        public int TopicId { get; set; }
        public int SubjectId { get; set; }
        public int QuestionId { get; set; }

    }

}
