using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;
using ExamSystem.Extensions;

namespace ExamSystem.Data.Services
{
    public class BookService : EntityBaseRepository<Books>, IBookService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //public string webRootPath = _webHostEnvironment.WebRootPath;

        public BookService(AppDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public async Task AddNewBook(Books data, IFormFileCollection files)
        {
            // upload the file
            if (files != null && data != null)
            {
                //File path
                string fileUpload = _webHostEnvironment.WebRootPath + WC.PDF_Book_Path;
                // pass the files object to method to save and create files in directory
                var fileName = FileUploadAndConvert.UploadFileAndConvertToImage(files,fileUpload);

                var obj = new Books()
                {
                    Title = data.Title,
                    Code = data.Code,
                    Status = data.Status,
                    FileName = fileName + ".pdf",
                    FileThumbnail = fileName + ".jpeg",
                    Description = data.Description,
                    BookType = data.BookType,
                    CreatedOn = DateTime.Now.Date,
                    CreatedBy = data.CreatedBy,
                };

                //sve the data to the database
                await _context.Books.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBook(int id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string fileUpload = webRootPath + WC.PDF_Book_Path;
            // delete the choice table entry
            var Obj = await _context.Books.FirstOrDefaultAsync(n => n.Id == id);
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

        public async Task<List<Books>> GetAllBooks()
        {
            var responce = await _context.Books.Where(n => n.Status == Status.Active).ToListAsync();
            return responce;
        }

        public async Task<List<Books>> GetAllBooksByUser(string Id)
        {
            var responce = await _context.Books.Where(n => n.CreatedBy == Id).ToListAsync();
            return responce;
        }

        public async Task<List<Books>> GetAllBooksByStatus(Status status)
        {
            var responce = await _context.Books.Where(n => n.Status == status).ToListAsync();
            return responce;
        }

        public async Task<Books> GetBookById(int id)
        {
            var responce = await _context.Books.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        public async Task UpdateBook(int Id, Books data, IFormFileCollection files)
        {
            var Obj = await _context.Books.FirstOrDefaultAsync(n => n.Id == Id);

            //grab the file
            string Upload = _webHostEnvironment.WebRootPath + WC.PDF_Book_Path;
            var oldfile = Path.Combine(Upload, data.FileName);
            var oldImage = Path.Combine(Upload, data.FileThumbnail);

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
                Obj.BookType = data.BookType;
                Obj.Description = data.Description;
                Obj.UpdatedOn = DateTime.Now.Date;
                Obj.UpdatedBy = data.UpdatedBy;

                //update the data to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}

