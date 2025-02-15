﻿using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ExamSystem.Models;

namespace ExamSystem.Data.Services
{
    public class QuizService : EntityBaseRepository<Quiz>, IQuizService
    {
        private readonly AppDbContext _context;
        public QuizService(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Quiz>> GetAllQuizesByUser(string Id)
        {
            var responce = await _context.Quizes.Where(n => n.CreatedBy == Id).ToListAsync();
            return responce;
        }

        public async Task<QuizViewVM> RenderQuiz(int selectedClass, int selectedSubject, int mcqCount)
        {

            // get the questions
            List<QnAs>? objQnA = new();

            if (mcqCount.Equals(null))
            { mcqCount = 10; }

            var topics = await _context.Topics.Where(t => t.SubjectId == selectedSubject).ToListAsync();
            mcqCount =  mcqCount/topics.Count; //distribute the count to each chapter
            // for objective questions
            foreach (var Titem in topics)
            {
                // obejctive qustions

                var questions = await _context.Questions.Where(q => q.TopicId == Titem.Id).Where(t => t.QuestionType == QuestionTypes.MCQ).Take(mcqCount).OrderBy(o => Guid.NewGuid()).ToListAsync();
                foreach (var Qitem in questions)
                {
                    QnAs? varQnA = new();
                    varQnA.OptionsQnA = new Choice();

                    varQnA.QuestionID = Qitem.Id;
                    varQnA.QuestionTextL = Qitem.QuestionTextL;
                    varQnA.QuestionText = Qitem.QuestionText;
                    varQnA.QuestionType = Qitem.QuestionType;

                    //var options = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).Select(o => new { OptionID = o.Id, Option1 = o.Choice1, OptionL1 = o.ChoiceL1, Option2 = o.Choice2, OptionL2 = o.ChoiceL2, Option3 = o.Choice3, OptionL3 = o.ChoiceL3, Option4 = o.Choice4, OptionL4 = o.ChoiceL4, Answer = o.Answer }).ToListAsync();
                    //varQnA.OptionsQnA = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    //objQnA.Add(varQnA);
                    var ans = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    varQnA.OptionsQnA.ChoiceL1 = ans.ChoiceL1;
                    varQnA.OptionsQnA.ChoiceL2 = ans.ChoiceL2;
                    varQnA.OptionsQnA.ChoiceL3 = ans.ChoiceL3;
                    varQnA.OptionsQnA.ChoiceL4 = ans.ChoiceL4;
                    varQnA.OptionsQnA.Choice1 = ans.Choice1;
                    varQnA.OptionsQnA.Choice2 = ans.Choice2;
                    varQnA.OptionsQnA.Choice3 = ans.Choice3;
                    varQnA.OptionsQnA.Choice4 = ans.Choice4;
                    varQnA.OptionsQnA.Answer = ans.Answer;
                    varQnA.OptionsQnA.AnswerL = ans.AnswerL;

                    objQnA.Add(varQnA);
                }
            }

            QuizViewVM quizview = new QuizViewVM()
            {
                QuizMcqs = objQnA,
                
            };
            return quizview;
        }

        public async Task<Quiz> GetQuizesById(int Id)
        {
            var responce = await _context.Quizes.Where(n => n.Id == Id).FirstOrDefaultAsync();
            return responce;
        }
    }
}
