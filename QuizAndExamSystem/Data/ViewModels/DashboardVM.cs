using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class DashboardVM
    {
        public List<SED>? Sed { get; set; }
        public List<Question>? Questions { get; set; }
        public List<Topic>? Topics { get; set; }
        public List<Books>? Books { get; set; }
        public List<Subject>? Subjects { get; set; }
        public List<Grade>? Grades { get; set; }
        public List<GeneratedPaper>? Papers { get; set; }
        public List<Quiz>? Quizes { get; set; }
        public List<ApplicationUser>? Users { get; set; }
        public List<Uploads>? Uploads { get; set; }

        // refered to each user's data 
        public List<SED>? MySed { get; set; }
        public List<Question>? MyQuestions { get; set; }
        public List<Topic>? MyTopics { get; set; }
        public List<Books>? MyBooks { get; set; }
        public List<Subject>? MySubjects { get; set; }
        public List<Grade>? MyGrades { get; set; }
        public List<GeneratedPaper>? MyPapers { get; set; }
        public List<Quiz>? MyQuizes { get; set; }
        public List<Uploads>? MyUploads { get; set; }
    }
}
