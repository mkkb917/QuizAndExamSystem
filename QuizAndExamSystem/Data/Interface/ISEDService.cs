using ExamSystem.Data.Base;
using ExamSystem.Data.Static;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface ISEDService : IEntityBaseRepository<SED>
    {
        Task<SED> GetFileById(int id);
        Task<List<SED>> GetAllFilesByUser(string Id);
        Task<List<SED>> GetAllFilesByStatus(Status status);
        Task<List<SED>> GetAllFiles();
        Task AddNewFile(SED data, IFormFileCollection files);
        Task UpdateFile(int Id, SED data, IFormFileCollection files);
        Task DeleteFile(int id);
    }
}
