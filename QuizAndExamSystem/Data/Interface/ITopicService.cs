using ExamSystem.Data.Base;
using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface ITopicService : IEntityBaseRepository<Topic>
    {
        Task<DropDownsListsVM> GetSubjectList();
        Task<Subject> GetSubjectById(int id);
        Task<List<Topic>> GetAllTopicsById(int Id);
        Task<List<Question>> GetAllQuestionsById(int Id);
        Task AddNewTopic(TopicsVM data);
        Task UpdateTopic(int Id, TopicsVM data);

    }
}
