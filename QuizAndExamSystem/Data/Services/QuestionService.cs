using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;

namespace ExamSystem.Data.Services
{
    public class QuestionService : EntityBaseRepository<Question>, IQuestionService
    {
        private readonly AppDbContext _context;
        public QuestionService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewQuestion(QuestionVM data)
        {
            // Question data
            var newQuestion = new Question()
            {
                TopicId = data.TopicId,
                Code = data.Code,
                Status = data.Status,
                QuestionText = data.QuestionText,
                QuestionTextL = data.QuestionTextL,
                QuestionType = data.QuestionType,
                DifficultyLevel = data.DifficultyLevel,
                Description = data.Description,
                CreatedBy = data.CreatedBy,
                CreatedOn = data.CreatedOn,
                UpdatedBy = data.UpdatedBy,
                UpdatedOn = data.UpdatedOn,

            };
            await _context.Questions.AddAsync(newQuestion);
            await _context.SaveChangesAsync();

            // Choices data
            var newAnswer = new Choice()
            {
                QuestionId = newQuestion.Id,
                Choice1 = data.ChoiceTitle1,
                ChoiceL1 = data.ChoiceTitleL1,
                Choice2 = data.ChoiceTitle2,
                ChoiceL2 = data.ChoiceTitleL2,
                Choice3 = data.ChoiceTitle3,
                ChoiceL3 = data.ChoiceTitleL3,
                Choice4 = data.ChoiceTitle4,
                ChoiceL4 = data.ChoiceTitleL4,
                Answer = data.CorrectAnswer,
                AnswerL = data.CorrectAnswerL,
                Description = data.Description,
            };
            await _context.Choices.AddAsync(newAnswer);
            await _context.SaveChangesAsync();

            // QuestionMeta data
            //var newMeta = new QuestionMeta()
            //{
            //    QuestionId = newQuestion.Id,
            //    BoardName = data.BoardName,
            //    ExamName = data.ExamName,
            //    ExamYear = data.ExamYear,
            //    Session = data.SessionId,
            //    Keywords = data.Keywords,
            //};
            //await _context.QuestionMetas.AddAsync(newMeta);
            //await _context.SaveChangesAsync();
        }

        public async Task<DropDownsListsVM> GetDDLObject()
        {
            var responce = new DropDownsListsVM()
            {
                Grades = await _context.Grades.Where(s => s.Status == Status.Active).OrderBy(t => t.GradeText).ToListAsync(),
                Subjects = await _context.Subjects.Where(s => s.Status == Status.Active).OrderBy(t => t.SubjectText).ToListAsync(),
                Topics = await _context.Topics.Where(s => s.Status == Status.Active).OrderBy(t => t.TopicText).ToListAsync()
            };
            return responce;
        }

        public async Task<Question> GetQuesDetailById(int id)
        {
            var responce = await _context.Questions
                .Include(c => c.Choice)
                .Include(b => b.QuestionMeta).FirstOrDefaultAsync(i => i.Id == id);
            return responce;
        }

        public async Task<Topic> GetTopicById(int id)
        {
            var responce = await _context.Topics.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }
        public async Task<Subject> GetSubjectById(int id)
        {
            var responce = await _context.Subjects.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        public async Task<List<Question>> GetAllQuestionsById(int Id)
        {
            var responce = await _context.Questions.Where(s => s.TopicId == Id).ToListAsync();
            return responce;
        }
        public async Task<List<Question>> GetAllQuestionsByStatus(Status status)
        {
            var responce = await _context.Questions.Where(s => s.Status == status).ToListAsync();
            return responce;
        }

        public async Task UpdateQuestion(int Id, QuestionVM data)
        {
            var ObjQuestion = await _context.Questions.FirstOrDefaultAsync(n => n.Id == Id);
            if (ObjQuestion != null)
            {
                // Question data
                ObjQuestion.Code = data.Code;
                ObjQuestion.Status = data.Status;
                ObjQuestion.QuestionText = data.QuestionText;
                ObjQuestion.QuestionTextL = data.QuestionTextL;
                ObjQuestion.QuestionType = data.QuestionType;
                ObjQuestion.DifficultyLevel = data.DifficultyLevel;
                ObjQuestion.Description = data.Description;
                ObjQuestion.UpdatedBy = data.UpdatedBy;
                ObjQuestion.UpdatedOn = data.UpdatedOn;

                //commet changes to database
                await _context.SaveChangesAsync();
            }
            var ObjChoice = await _context.Choices.FirstOrDefaultAsync(n => n.QuestionId == Id);
            if (ObjChoice != null)
            {
                // Choices data
                ObjChoice.Choice1 = data.ChoiceTitle1;
                ObjChoice.ChoiceL1 = data.ChoiceTitleL1;
                ObjChoice.Choice2 = data.ChoiceTitle2;
                ObjChoice.ChoiceL2 = data.ChoiceTitleL2;
                ObjChoice.Choice3 = data.ChoiceTitle3;
                ObjChoice.ChoiceL3 = data.ChoiceTitleL3;
                ObjChoice.Choice4 = data.ChoiceTitle4;
                ObjChoice.ChoiceL4 = data.ChoiceTitleL4;
                ObjChoice.Answer = data.CorrectAnswer;
                ObjChoice.AnswerL = data.CorrectAnswerL;
                ObjChoice.Description = data.Description;

                //commet changes to database
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteQuestion(int Id)
        {

            // delete the choice table entry
            var ObjChoice = await _context.Choices.FirstOrDefaultAsync(n => n.QuestionId == Id);
            if (ObjChoice != null)
            {
                _context.Remove(ObjChoice);
                await _context.SaveChangesAsync();
            }

            //delete teh metaquestion tabel entries
            System.Collections.Generic.List<QuestionMeta> ObjMeta = await _context.QuestionMetas.Where(n => n.QuestionId == Id).ToListAsync();
            if (ObjMeta != null)
            {
                _context.QuestionMetas.RemoveRange(ObjMeta);
                await _context.SaveChangesAsync();
            }
            // delete the question table entry
            var ObjQuestion = await _context.Questions.FirstOrDefaultAsync(n => n.Id == Id);
            if (ObjQuestion != null)
            {
                _context.Remove(ObjQuestion);
                await _context.SaveChangesAsync();
            }
        }





        //generate random questions from db based on given subject id and topic ids
        public async Task<List<QnAs>> GetQuestionBank(int? SubjectId, List<Topic>? Topics)
        {
            PaperSetting Objmodel = new PaperSetting();
            QnAs varQnA = null;
            var objQnA = new List<QnAs>();

            DifficultyLevel dl = Objmodel.DifficultyLevel;
            //var Qcount = Objmodel.TotalMarks;

            List<OptionsDetails> _objOlst = new List<OptionsDetails>();
            string? _subjectName = await _context.Subjects.Where(e => e.Id == SubjectId).Select(o => o.SubjectText).SingleOrDefaultAsync();

            if (Topics != null)
            {
                var questions = await _context.Questions.Where(q => q.Id == Convert.ToInt32(Topics)).OrderBy(t => t.DifficultyLevel == dl).ToListAsync();
                //var questions = await _context.Questions.Where(q => q.Id == _objT.TopicId).OrderBy(t => t.DifficultyLevel == dl).Take(Qcount).ToListAsync();


                foreach (var Qitem in questions)
                {

                    varQnA.QuestionID = Qitem.Id;
                    varQnA.QuestionTextL = Qitem.QuestionTextL;
                    varQnA.QuestionText = Qitem.QuestionText;

                    var options = await _context.Choices.Where(q => q.Id == Qitem.Id).Select(o => new { OptionID = o.Id, Option1 = o.Choice1, OptionL1 = o.ChoiceL1, Option2 = o.Choice2, OptionL2 = o.ChoiceL2, Option3 = o.Choice3, OptionL3 = o.ChoiceL3, Option4 = o.Choice4, OptionL4 = o.ChoiceL4, Answer = o.Answer }).ToListAsync();
                    foreach (var Oitem in options)
                    {
                        OptionsDetails _objO = new OptionsDetails()
                        {
                            OptionID = Oitem.OptionID,
                            Option1 = Oitem.Option1,
                            OptionL1 = Oitem.OptionL1,
                            Option2 = Oitem.Option2,
                            OptionL2 = Oitem.OptionL2,
                            Option3 = Oitem.Option3,
                            OptionL3 = Oitem.OptionL3,
                            Option4 = Oitem.Option4,
                            OptionL4 = Oitem.OptionL4,
                            Answer = Oitem.Answer
                        };
                        _objOlst.Add(_objO);
                    }
                    //varQnA.OptionsQnA = _objOlst;
                    objQnA.Add(varQnA);
                }

            }
            else
            {
                var questions = await _context.Questions.OrderBy(t => t.DifficultyLevel == dl).ToListAsync();

                foreach (var Qitem in questions)
                {
                    //QuestionDetails _objQ = new QuestionDetails();
                    varQnA.QuestionID = Qitem.Id;
                    varQnA.QuestionTextL = Qitem.QuestionTextL;
                    varQnA.QuestionText = Qitem.QuestionText;
                    //_objQ.QuestionDifficulty = dl;
                    //_objQ.QuestionType = Qitem.QuestionType;

                    var options = await _context.Choices.Where(q => q.Id == Qitem.Id).Select(o => new { OptionID = o.Id, Option1 = o.Choice1, OptionL1 = o.ChoiceL1, Option2 = o.Choice2, OptionL2 = o.ChoiceL2, Option3 = o.Choice3, OptionL3 = o.ChoiceL3, Option4 = o.Choice4, OptionL4 = o.ChoiceL4, Answer = o.Answer }).ToListAsync();
                    foreach (var Oitem in options)
                    {
                        OptionsDetails _objO = new OptionsDetails()
                        {
                            OptionID = Oitem.OptionID,
                            Option1 = Oitem.Option1,
                            OptionL1 = Oitem.OptionL1,
                            Option2 = Oitem.Option2,
                            OptionL2 = Oitem.OptionL2,
                            Option3 = Oitem.Option3,
                            OptionL3 = Oitem.OptionL3,
                            Option4 = Oitem.Option4,
                            OptionL4 = Oitem.OptionL4,
                            Answer = Oitem.Answer
                        };
                        _objOlst.Add(_objO);
                    }
                    //varQnA.OptionsQnA = _objOlst;
                    objQnA.Add(varQnA);
                }

            }
            return objQnA;
        }

        public async Task<bool> SearchQuestionByString(string questionString)
        {
            var responce = await _context.Questions
                .Where(e => e.QuestionText == questionString).FirstOrDefaultAsync();
            if (responce == null)
                return false;
            return true;
        }

        public async Task<List<QuestionMeta>> GetQuestionMetaById(int id)
        {
            var responce = await _context.QuestionMetas.Where(q => q.QuestionId == id).ToListAsync();
            return responce;
        }

        public async Task AddNewQuestionMeta(QuestionMeta data)
        {
            await _context.QuestionMetas.AddAsync(data);
            await _context.SaveChangesAsync();
        }
    }
}
