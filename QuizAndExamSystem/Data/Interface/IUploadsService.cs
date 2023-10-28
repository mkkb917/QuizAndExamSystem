using ExamSystem.Data.Base;
using ExamSystem.Data.Static;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IUploadsService : IEntityBaseRepository<Uploads>
    {
        // General Signatures
        Task<Uploads> GetFileById(int id, UploadsCategory category);
        Task<List<Uploads>> GetAllFilesByUser(string user, UploadsCategory category);
        Task<List<Uploads>> GetAllFilesByCategory(Status status, UploadsCategory code);
        Task<List<Uploads>> GetAllFilesByStatus(Status status);
        Task AddNewFile(Uploads data, IFormFileCollection files);
        Task UpdateFile(int Id, Uploads data, IFormFileCollection files);
        Task ApproveFile(int id);
        Task DeleteFile(int id);
        

        
    }
}
