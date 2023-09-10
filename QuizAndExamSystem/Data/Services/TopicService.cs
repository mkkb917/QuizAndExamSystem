using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;

namespace ExamSystem.Data.Services
{
    public class TopicService : EntityBaseRepository<Topic>, ITopicService
    {
        //pass the datacontext object to base class
        private readonly AppDbContext _context;
        public TopicService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewTopic(TopicsVM data)
        {
            var newTopic = new Topic()
            {
                Code = data.Code,
                TopicText = data.TopicText,
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
    }
}
