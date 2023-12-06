using ExamSystem.Data.Base;
using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface ISubjectService : IEntityBaseRepository<Subject>
    {
        // get the grade Id for creating new Subject entry into database
        Task<DropDownsListsVM> GetGradesList();
        Task<Grade> GetGradeById(int id);
        Task<Subject> GetSubjectById(int id);
        Task<List<Subject>> GetAllActiveSubjectsById(int Id);
        Task<List<Topic>> GetAllTopicsById(int Id);
        Task AddNewSubject(SubjectsVM data);
        Task UpdateSubject(int Id, SubjectsVM data);
        Task DeleteSubject(int id);

        string DeleteOldAndUploadNewFile(IFormFileCollection files, string? oldFile);
        void DeleteFile(string fileName);

    }
}
