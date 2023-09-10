using Aspose.Pdf;
using Aspose.Pdf.Devices;
using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ExamSystem.Models;

namespace ExamSystem.Data.Services
{
    public class UploadsService : EntityBaseRepository<Uploads>, IUploadsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //public string webRootPath = _webHostEnvironment.WebRootPath;

        public UploadsService(AppDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public async Task AddNewFile(Uploads data, IFormFileCollection files)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string Upload = "";

            // check the file type by model.category
            if (data.FileType == UploadsCategory.Syllabus)    //Syllabus =1
            {
                Upload = webRootPath + WC.UploadSyllabusPathPDF;
            }
            else if (data.FileType == UploadsCategory.PastPapers)   // PastPapers =2
            {
                Upload = webRootPath + WC.UploadPaperPathPDF;
            }
            else if (data.FileType == UploadsCategory.Notes)     //Notes =3
            {
                Upload = webRootPath + WC.UploadNotesPathPDF;
            }
            else if (data.FileType == UploadsCategory.Books)     //Books =4
            {
                Upload = webRootPath + WC.UploadBooksPathPDF;
            }

            // upload the file
            if (files != null && data != null)
            {
                // pass the files object to method to save and create files in directory
                var fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload);

                var obj = new Uploads()
                {
                    Title = data.Title,
                    Code = data.Code,
                    Status = data.Status,
                    Author = data.Author,
                    FileName = fileName + ".pdf",
                    FileThumbnail = fileName + ".jpeg",
                    Description = data.Description,
                    FileType = data.FileType,
                    CreatedOn = DateTime.Now,
                    CreatedBy = data.CreatedBy,
                };

                //sve the data to the database
                await _context.Uploads.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFile(int id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileUpload = "";
            // delete the choice table entry
            var Obj = await _context.Uploads.FirstOrDefaultAsync(n => n.Id == id);

            // check the file type by model.category
            if (Obj.FileType == UploadsCategory.Syllabus)    //Syllabus =1
            {
                fileUpload = webRootPath + WC.UploadSyllabusPathPDF;
            }
            else if (Obj.FileType == UploadsCategory.PastPapers)   // PastPapers =2
            {
                fileUpload = webRootPath + WC.UploadPaperPathPDF;
            }
            else if (Obj.FileType == UploadsCategory.Notes)     //Notes =3
            {
                fileUpload = webRootPath + WC.UploadNotesPathPDF;
            }
            else if (Obj.FileType == UploadsCategory.Books)     //Books =4
            {
                fileUpload = webRootPath + WC.UploadBooksPathPDF;
            }
            if (Obj != null)
            {
                // delete the record
                _context.Remove(Obj);
                await _context.SaveChangesAsync();

                // delete the uploaded file from directory
                //fetch the file name and path
                var fileName = fileUpload + Obj.FileName;
                var fileImageFace = fileUpload + Obj.FileThumbnail;

                if (fileName != null || fileName != string.Empty)
                {
                    if (System.IO.File.Exists(fileName))
                        System.IO.File.Delete(fileName);
                }
                // delete the image from directory
                if (fileImageFace != null || fileImageFace != string.Empty)
                {
                    if (System.IO.File.Exists(fileImageFace))
                        System.IO.File.Delete(fileImageFace);
                }
            }
        }

        public async Task<List<Uploads>> GetAllFiles(string code)
        {
            var responce = await _context.Uploads.Where(n => n.Status == Status.Active).Where(c=>c.Code == code).ToListAsync();
            return responce;
        }

        public async Task<List<Uploads>> GetAllFilesByStatus(Status status, string code)
        {
            var responce = await _context.Uploads.Where(n => n.Status == status).Where(c => c.Code == code).ToListAsync();
            return responce;
        }

        public async Task<List<Uploads>> GetAllFilesByUser(string Id, string code)
        {
            var responce = await _context.Uploads.Where(n => n.CreatedBy == Id).Where(c => c.Code == code).ToListAsync();
            return responce;
        }

        public async Task<Uploads> GetFileById(int id, string code)
        {
            var responce = await _context.Uploads.Where(n => n.Id == id).Where(c => c.Code == code).FirstOrDefaultAsync();
            return responce;
        }

        public async Task UpdateFile(int Id, Uploads data, IFormFileCollection files)
        {
            var Obj = await _context.Uploads.FirstOrDefaultAsync(n => n.Id == Id);

            //grab the file
            string webRootPath = _webHostEnvironment.WebRootPath;
            string Upload = "";
            var oldfile = Path.Combine(Upload, data.FileName);
            var oldImage = Path.Combine(Upload, data.FileThumbnail);

            // check the file type by model.category
            if (data.FileType == UploadsCategory.Syllabus)    //Syllabus =1
            {
                Upload = webRootPath + WC.UploadSyllabusPathPDF;
            }
            else if (data.FileType == UploadsCategory.PastPapers)   // PastPapers =2
            {
                Upload = webRootPath + WC.UploadPaperPathPDF;
            }
            else if (data.FileType == UploadsCategory.Notes)     //Notes =3
            {
                Upload = webRootPath + WC.UploadNotesPathPDF;
            }
            else if (data.FileType == UploadsCategory.Books)     //Books =4
            {
                Upload = webRootPath + WC.UploadBooksPathPDF;
            }

            if (files.Count > 0)
            {
                //check if the file already exists then delete the old file
                if (File.Exists(oldfile) && File.Exists(oldImage))
                {
                    File.Delete(oldfile);
                    File.Delete(oldImage);
                }
                //call the method to upload new file and create image in directory
                var fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload);
                if (fileName != null)
                {
                    if (fileName != null)
                    {
                        Obj.FileName = fileName + ".pdf";
                        Obj.FileThumbnail = fileName + ".jpeg";
                    }
                }
            }
            if (Obj != null)
            {
                Obj.Title = data.Title;
                Obj.Code = data.Code;
                Obj.FileName = data.FileName;
                Obj.FileThumbnail = data.FileThumbnail;
                Obj.Status = data.Status;
                Obj.FileType = data.FileType;
                Obj.Description = data.Description;
                Obj.UpdatedOn = DateTime.Now.Date;
                Obj.UpdatedBy = data.UpdatedBy;

                //update the data to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}

