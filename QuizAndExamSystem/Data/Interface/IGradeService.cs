using ExamSystem.Data.Base;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IGradeService : IEntityBaseRepository<Grade>
    {
        Task<List<Subject>> GetSubjectById(int Id);
        Task<List<Subject>?> GetSubjectByGrade(Grade item);
        Task<List<Grade>?> GetActiveGradesByOrder();
        Task<bool> SearchGrade(string searchTerm);
        string DeleteOldAndUploadNewFile(IFormFileCollection files, string? oldFile);
        void DeleteFile(string fileName);
    }   
}
