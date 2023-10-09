using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IPdfService
    {
        Task<List<GeneratedPaper>> GetAllPapers();
        Task DeletePaper(int id);
        Task<GeneratedPaper> GetPaperById(int id);
        Task<List<GeneratedPaper>> GetAllPapersByUser(string Id);
        Task<PaperViewVM> RenderPaper(int selectedClass, int selectedSubject, DateTime PaperDate, string? TeacherName,string? qrCode ,ApplicationUser usr);
        Task<byte[]> RenderPdf(string paperView, string fileName);
        Task<bool> SavePdfAsync(string gradeName, string subjectName, string fileObjectivePaper, string fileSubjectivePaper, string fileSolution, ApplicationUser user, string qrcode);
        public string BarCodeGenerator(string QrCodeString, string guid);
        public void DeleteFile(string path, string Name);
    }
}
