using ExamSystem.Data.Base;
using ExamSystem.Data.ViewModels;
using System.Linq.Expressions;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IQuizService : IEntityBaseRepository<Quiz>
    {
        public Task<List<Quiz>> GetAllQuizesByUser(string Id);
        public Task<QuizViewVM> RenderQuiz(int selectedClass, int selectedSubject, int mcqCount);
        
    }
}
