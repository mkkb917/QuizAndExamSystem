using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;
using ExamSystem.Extensions;
using ExamSystem.Data.Static;

namespace ExamSystem.Data.Services
{
    public class TopicService : EntityBaseRepository<Topic>, ITopicService
    {
        //pass the datacontext object to base class
        private readonly AppDbContext _context;
        private readonly string webRootPath;
        public TopicService(AppDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            webRootPath = webHostEnvironment.WebRootPath;
        }

        public async Task AddNewTopic(TopicsVM data)
        {
            var newTopic = new Topic()
            {
                Code = data.Code,
                TopicText = data.TopicText,
                Image = data.Image,
                Status = data.Status,
                SubjectId = data.SubjectId,
                Description = data.Description,
                CreatedOn = DateTime.Now.Date,
                CreatedBy = data.user,
                MCQMarks = data.MCQMarks,
                MCQCount = data.MCQCount,
                SEQMarks = data.SEQMarks,
                SEQCount = data.SEQCount,
                LongQMarks = data.LongQMarks,
                LongQCount = data.LongQCount,
            };

            //sve the data to the database
            await _context.Topics.AddAsync(newTopic);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetAllQuestionsById(int Id)
        {
            var responce = await _context.Questions.Where(s => s.TopicId == Id).ToListAsync();
            return responce;
        }

        // Get list of topics by subject Id
        public async Task<List<Topic>> GetAllTopicsById(int Id)
        {
            var responce = await _context.Topics.Where(s => s.SubjectId == Id).ToListAsync();
            return responce;
        }

        public async Task<Subject> GetSubjectById(int id)
        {
            var responce = await _context.Subjects.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        public async Task<DropDownsListsVM> GetSubjectList()
        {
            var responce = new DropDownsListsVM()
            {
                Subjects = await _context.Subjects.ToListAsync()
            };

            return responce;
        }

        public async Task UpdateTopic(int Id, TopicsVM data)
        {
            var obj = await _context.Topics.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (obj != null)
            {
                obj.Code = data.Code;
                obj.TopicText = data.TopicText;
                obj.Image = data.Image;
                obj.Status = data.Status;
                obj.Description = data.Description;
                obj.UpdatedOn = DateTime.Now.Date;
                obj.UpdatedBy = data.user;
                obj.MCQMarks = data.MCQMarks;
                obj.MCQCount = data.MCQCount;
                obj.SEQMarks = data.SEQMarks;
                obj.SEQCount = data.SEQCount;
                obj.LongQMarks = data.LongQMarks;
                obj.LongQCount = data.LongQCount;
                //update the data to the database
                await _context.SaveChangesAsync();

            }
        }

        public string DeleteOldAndUploadNewFile(IFormFileCollection files, string? oldFile)
        {
            string Upload = webRootPath + WC.TopicImagePath;

            // pass the file name, path and file to uploader
            string fileName = Path.GetFileNameWithoutExtension(files[0].FileName);
            //if the file already exsit then delete it before new upload
            if (oldFile != null)
            {
                //delete the old file
                var oldfile = Path.Combine(Upload, oldFile);
                //check if the file already exists then delete the old file
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);

                }
            }
            // pass the new files object to funtion to save file and create thumbnail in directory
            var uploadImage = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload, fileName);
            return uploadImage;
        }
        public void DeleteFile(string fileName)
        {
            string Upload = webRootPath + WC.TopicImagePath;

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
