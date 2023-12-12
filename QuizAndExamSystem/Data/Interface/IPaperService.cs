using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IPaperService
    {
        Task<List<GeneratedPaper>> GetAllPapers();
        Task DeletePaper(int id);
        Task<GeneratedPaper> GetPaperById(int id);
        Task<List<GeneratedPaper>> GetAllPapersByUser(string Id);
        Task<PaperViewVM> RenderPaper(int selectedClass, int selectedSubject, List<int>? selectedTopics, DateTime PaperDate, string? TeacherName,string qrCode ,ApplicationUser usr);
        Task<byte[]> RenderPdf(string paperView, string fileName);
        Task<bool> SavePdfAsync(string gradeName, string subjectName, string fileObjectivePaper, string fileSubjectivePaper, string fileSolution, ApplicationUser user, string qrcode);
        Task<PaperSetting> GetPaperSettingByUser(ApplicationUser user);
        Task<List<GeneratedPaper>> GetAllPapersByUserAndDate(string createdBy, DateTime date);
        Task<PaperSetting> GetPaperSettingById(int? Id);
        Task CreatePaperSetting(PaperSetting paperSetting);
        Task UpdatePaperSetting(int Id,PaperSetting paperSetting);
        
        public string BarCodeGenerator(string QrCodeString, string guid);
        public void DeleteFile(string path, string Name);
    }
}
