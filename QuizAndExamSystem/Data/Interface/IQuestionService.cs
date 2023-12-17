using ExamSystem.Data.Base;
using ExamSystem.Data.Static;
using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IQuestionService : IEntityBaseRepository<Question>
    {
        Task<List<QnAs>> GetQuestionBank(int? SubjectId, List<Topic>? Topics);
        Task<Question> GetQuesDetailById(int id);
        Task<QuestionMeta> GetQuestionMetaById(int id);
        Task<bool> SearchQuestionByString(string questionString);
        Task<List<Question>> GetAllQuestionsById(int Id);
        Task<List<Question>> GetAllQuestionsByStatus(Status status);
        Task<DropDownsListsVM> GetDDLObject();
        Task AddNewQuestion(QuestionVM data);
        Task AddNewQuestionMeta(QuestionMeta data);
        Task UpdateQuestion(int id, QuestionVM data);
        Task DeleteQuestion(int id);
        Task DeleteQuestionMeta(int id);

        Task<Topic> GetTopicById(int id);
        Task<Subject> GetSubjectById(int id);

    }
}
