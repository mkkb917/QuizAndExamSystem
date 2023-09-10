using Aspose.Pdf;
using Aspose.Pdf.Devices;
using ExamSystem.Data.Base;
using ExamSystem.Data.Static;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IUploadsService : IEntityBaseRepository<Uploads>
    {
        // General Signatures
        Task<Uploads> GetFileById(int id, string code);
        Task<List<Uploads>> GetAllFilesByUser(string user, string code);
        Task<List<Uploads>> GetAllFilesByStatus(Status status, string code);
        Task<List<Uploads>> GetAllFiles(string code);
        Task AddNewFile(Uploads data, IFormFileCollection files);
        Task UpdateFile(int Id, Uploads data, IFormFileCollection files);
        Task DeleteFile(int id);
        

        
    }
}
