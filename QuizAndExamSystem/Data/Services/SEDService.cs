using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;
using ExamSystem.Extensions;

namespace ExamSystem.Data.Services
{
    public class SEDService : EntityBaseRepository<SED>, ISEDService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //public string webRootPath = _webHostEnvironment.WebRootPath;

        public SEDService(AppDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
       
        public async Task AddNewFile(SED data, IFormFileCollection files)
        {
            string NameofFile = Path.GetFileName(files[0].FileName);
            // upload the file
            if (files != null && data != null)
            {
                string fileUpload = _webHostEnvironment.WebRootPath + WC.PDF_SED_Path;
                // pass the files object to method to save and create files in directory

                var fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files, fileUpload,NameofFile);

                var obj = new SED()
                {
                    Title = data.Title,
                    Code = data.Code,
                    IssuedBy = data.IssuedBy,
                    Status = data.Status,
                    FileName = fileName + ".pdf",
                    FileFaceImage = fileName + ".jpeg",
                    Date = data.Date,
                    Description = data.Description,
                    CreatedOn = DateTime.Now.Date,
                    CreatedBy = data.CreatedBy,
                };

                //save the data to the database
                await _context.SED.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFile(int id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileUpload = webRootPath + WC.PDF_SED_Path;
            // delete the choice table entry
            var Obj = await _context.SED.FirstOrDefaultAsync(n => n.Id == id);
            if (Obj != null)
            {
                // delete the record
                _context.Remove(Obj);
                await _context.SaveChangesAsync();

                // delete the uploaded file from directory
                //fetch the file name and path
                var fileName = fileUpload + Obj.FileName;
                var fileImageFace = fileUpload + Obj.FileFaceImage;

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

        public async Task<List<SED>> GetAllFiles()
        {
            var responce = await _context.SED.Where(n=>n.Status == Status.Active).ToListAsync();
            return responce;
        }

        public async Task<List<SED>> GetAllFilesByUser(string Id)
        {
            var responce = await _context.SED.Where(n => n.CreatedBy == Id).ToListAsync();
            return responce;
        }

        public async Task<List<SED>> GetAllFilesByStatus(Status status)
        {
            var responce = await _context.SED.Where(n => n.Status == status).ToListAsync();
            return responce;
        }

        
        public async Task<SED> GetFileById(int id)
        {
            var responce = await _context.SED.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        public async Task UpdateFile(int Id, SED data, IFormFileCollection files)
        {
            var Obj = await _context.SED.FirstOrDefaultAsync(n => n.Id == Id);
            string NameofFile = Path.GetFileName(files[0].FileName);
            //grab the file
            string Upload = _webHostEnvironment.WebRootPath + WC.PDF_SED_Path;
            var oldfile = Path.Combine(Upload, data.FileName);
            var oldImage = Path.Combine(Upload, data.FileFaceImage);

            if (files.Count > 0)
            {
                //check if the file already exists then delete the old file
                if (File.Exists(oldfile) && File.Exists(oldImage))
                {
                    File.Delete(oldfile);
                    File.Delete(oldImage);
                }
                //call the method to upload new file and create image in directory
                var fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files,Upload, NameofFile);
                if (fileName != null)
                {
                    if (fileName != null)
                    {
                        Obj.FileName = fileName + ".pdf";
                        Obj.FileFaceImage = fileName + ".jpeg";
                    }
                }
            }
            if (Obj != null)
            {
                Obj.Title = data.Title;
                Obj.Code = data.Code;
                Obj.IssuedBy = data.IssuedBy;
                Obj.FileName = data.FileName;
                Obj.FileFaceImage = data.FileFaceImage;
                Obj.Status = data.Status;
                Obj.Description = data.Description;
                Obj.UpdatedOn = DateTime.Now.Date;
                Obj.UpdatedBy = data.UpdatedBy;

                //update the data to the database
                await _context.SaveChangesAsync();
            }
        }

    }
}

