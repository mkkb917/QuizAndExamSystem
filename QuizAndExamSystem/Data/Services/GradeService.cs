using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Extensions;
using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Services
{
    // inherits the class from general Entity base repository by passing the Grade class entity
    public class GradeService:EntityBaseRepository<Grade>, IGradeService
    {
         
        private readonly AppDbContext _context;
        private readonly string webRootPath;
        public GradeService(AppDbContext context, IWebHostEnvironment webHostEnvironment) :base(context)
        {
            _context = context;
            webRootPath = webHostEnvironment.WebRootPath;
        }

        

        public async Task<List<Subject>> GetSubjectById(int Id)
        {
            var responce = await _context.Subjects.Where(s => s.GradeId == Id).ToListAsync();
            return responce;
        }

        public async Task<List<Subject>?> GetSubjectByGrade(Grade item)
        {
            var responce = await _context.Subjects.Where(s => s.Grade == item).ToListAsync();
            return responce;
        }
        public async Task<bool>SearchGrade(string searchTerm)
        {
            var responce = await _context.Grades.Where(s=>s.GradeText==searchTerm).FirstOrDefaultAsync();
            if (responce!=null) return true;
            return false;
        }

        public string DeleteOldAndUploadNewFile(IFormFileCollection files, string? oldFile)
        {
            string Upload = webRootPath + WC.GradeImagePath;

            // pass the file name, path and file to uploader
            string fileName = Path.GetFileNameWithoutExtension(files[0].FileName);
            //if the file already exsit then delete it before new upload
            //delete the old file
            var oldfile = Path.Combine(Upload, oldFile);
            //check if the file already exists then delete the old file
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);

            }
            // pass the new files object to funtion to save file and create thumbnail in directory
            var uploadImage = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload, fileName);
            return uploadImage;
        }
        public void DeleteFile(string fileName)
        {
            string Upload = webRootPath + WC.GradeImagePath;

            //delete the old file
            var oldfile = Path.Combine(Upload, fileName);
            //check if the file already exists then delete the old file
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
                
            }
            
        }
    }
}
