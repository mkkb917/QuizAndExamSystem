using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Extensions;
using ExamSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ExamSystem.Data.Services
{
    public class UploadsService : EntityBaseRepository<Uploads>, IUploadsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        //public string webRootPath = _webHostEnvironment.WebRootPath;

        public UploadsService(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task AddNewFile(Uploads data, IFormFileCollection files)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string Upload = "";

            // check the file type by model.category
            if (data.FileType == UploadsCategory.Syllabus)    //Syllabus =1
            {
                Upload = webRootPath + WC.UploadSyllabus;
            }
            else if (data.FileType == UploadsCategory.PastPapers)   // PastPapers =2
            {
                Upload = webRootPath + WC.UploadPastPaper;
            }
            else if (data.FileType == UploadsCategory.Notes)     //Notes =3
            {
                Upload = webRootPath + WC.UploadNotes;
            }
            else if (data.FileType == UploadsCategory.Decorate)     //Decorate = 4
            {
                Upload = webRootPath + WC.UploadDecorate;
            }
            else if (data.FileType == UploadsCategory.Events)     //Events = 5
            {
                Upload = webRootPath + WC.UploadEvents;
            }
            else if (data.FileType == UploadsCategory.Manage)     //manage = 6
            {
                Upload = webRootPath + WC.UploadManage;
            }
            else if (data.FileType == UploadsCategory.Calender)     //Calender = 7
            {
                Upload = webRootPath + WC.UploadCalender;
            }

            // upload the file
            if (files != null && data != null)
            {
                string fileExtension = Path.GetExtension(files[0].FileName);
                string fileName = Path.GetFileName(files[0].FileName);

                //create new randon name for file
                string GuidName = Guid.NewGuid().ToString()[..5];

                // pass the files object to funtion to save file and create thumbnail in directory
                fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload, GuidName);
                if (fileExtension == ".pdf")
                {
                    fileExtension = ".jpeg";
                }

                var obj = new Uploads();

                obj.Title = data.Title;
                obj.Code = data.Code;
                obj.Status = data.Status;
                obj.Author = data.Author;
                obj.FileName = GuidName + fileExtension;
                obj.FileThumbnail = GuidName + fileExtension;
                obj.Description = data.Description;
                obj.FileType = data.FileType;
                obj.CreatedOn = DateTime.Now;
                obj.CreatedBy = data.CreatedBy;


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
                fileUpload = webRootPath + WC.UploadSyllabus;
            }
            else if (Obj.FileType == UploadsCategory.PastPapers)   // PastPapers =2
            {
                fileUpload = webRootPath + WC.UploadPastPaper;
            }
            else if (Obj.FileType == UploadsCategory.Notes)     //Notes =3
            {
                fileUpload = webRootPath + WC.UploadNotes;
            }
            else if (Obj.FileType == UploadsCategory.Decorate)     //decoration =4
            {
                fileUpload = webRootPath + WC.UploadDecorate;
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

        public async Task<List<Uploads>> GetAllFilesByStatus(Status status)
        {
            var responce = await _context.Uploads.Where(n => n.Status == status).ToListAsync();
            return responce;
        }

        public async Task<List<Uploads>> GetAllFilesByCategory(Status status, UploadsCategory code)
        {
            var responce = await _context.Uploads.Where(n => n.Status == status).Where(c => c.FileType == code).ToListAsync();
            return responce;
        }

        public async Task<List<Uploads>> GetAllFilesByUser(string user, UploadsCategory category)
        {
            var currentUser = await _userManager.FindByNameAsync(user);
            List<Uploads> responce = new();
            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                // grap all record if the current user is admin user
                responce = await _context.Uploads.Where(c => c.FileType == category).ToListAsync();
                return responce;
            }
            responce = await _context.Uploads.Where(n => n.CreatedBy == user).Where(c => c.FileType == category).ToListAsync();
            return responce;
        }

        public async Task<List<Uploads>> GetFilesByCategoryForComponenet(Status status, UploadsCategory code)
        {
            var responce = await _context.Uploads.Where(n => n.Status == status).Where(c => c.FileType == code).Take(6).ToListAsync();
            return responce;
        }
        public async Task<Uploads> GetFileById(int id, UploadsCategory category)
        {
            var responce = await _context.Uploads.Where(n => n.Id == id).Where(c => c.FileType == category).FirstOrDefaultAsync();
            return responce;
        }

        public async Task UpdateFile(int Id, Uploads data, IFormFileCollection files)
        {
            var Obj = await _context.Uploads.FirstOrDefaultAsync(n => n.Id == Id);
            if (Obj == null)
            {
                // Handle the case where the specified file with the given Id doesn't exist.
                return;
            }

            string NameofFile = Path.GetFileName(files[0].FileName);
            //grab the file
            string webRootPath = _webHostEnvironment.WebRootPath;
            string Upload = "";


            // check the file type by model.category
            if (data.FileType == UploadsCategory.Syllabus)    //Syllabus =1
            {
                Upload = webRootPath + WC.UploadSyllabus;
            }
            else if (data.FileType == UploadsCategory.PastPapers)   // PastPapers =2
            {
                Upload = webRootPath + WC.UploadPastPaper;
            }
            else if (data.FileType == UploadsCategory.Notes)     //Notes =3
            {
                Upload = webRootPath + WC.UploadNotes;
            }


            if (files.Count > 0)
            {
                var oldfile = Path.Combine(Upload, Obj.FileName);
                var oldImage = Path.Combine(Upload, data.FileThumbnail);
                //check if the file already exists then delete the old file
                if (File.Exists(oldfile) && File.Exists(oldImage))
                {
                    File.Delete(oldfile);
                    File.Delete(oldImage);
                }
                //call the method to upload new file and create image in directory
                var fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload, NameofFile);
                if (fileName != null)
                {
                    if (fileName != null)
                    {
                        Obj.FileName = fileName + ".pdf";
                        Obj.FileThumbnail = fileName + ".jpeg";
                    }
                }
            }

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

        public async Task ApproveFile(int id)
        {
            // delete the choice table entry
            var Obj = await _context.Uploads.FirstOrDefaultAsync(n => n.Id == id);

            if (Obj != null)
            {
                Obj.Status = Status.Active;
                // delete the record
                _context.Update(Obj);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}

