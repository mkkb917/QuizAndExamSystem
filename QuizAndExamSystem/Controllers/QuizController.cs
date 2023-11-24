using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Data.ViewModels;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    [Authorize]
    public class QuizController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuizService _QuizService;
        private readonly ISubjectService _SubjectService;
        public QuizController(ISubjectService SubjectService, Data.AppDbContext context, UserManager<ApplicationUser> userManager, IQuizService QuizService)
        {
            _context = context;
            _userManager = userManager;
            _QuizService = QuizService;
            _SubjectService = SubjectService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var newQuestionDropdown = await _context.Grades.OrderBy(t => t.GradeText).ToListAsync();
            ViewBag.Grades = new SelectList(newQuestionDropdown, "Id", "GradeText");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuizView(string? GradeDDL, string? SubjectDDL)
        {
            // get the class and subject DDL selected value
            int selectedClass = Convert.ToInt32(GradeDDL);
            int selectedSubject = Convert.ToInt32(SubjectDDL);
            int count = 20;     // total number of quiz mcqs
                               // get current user and validate as User.Student type 

            //if ((User.IsInRole("Student") || (User.IsInRole("Admin"))) != true)
            //{
            //    TempData["error"] = "your are not authorize to take test. you are not a student user!.";
            //    return RedirectToAction("Create");
            //}

            // call the function to render the questions bank
            var quizViewVM = await _QuizService.RenderQuiz(selectedClass, selectedSubject, count);
            if (quizViewVM.QuizMcqs == null)
                return RedirectToAction("Create");

            // Pass user, Class and subject to view
            var gradeName = await _SubjectService.GetGradeById(selectedClass);
            var subjectName = await _SubjectService.GetSubjectById(selectedSubject);
            quizViewVM.UserName = _userManager.GetUserName(User);
            quizViewVM.TotalMarks = count;
            //string startTime = DateTime.Now.ToShortTimeString().ToString();
            //string endTime = startTime.AddMinutes(30);
            //quizViewVM.Duration =  DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
            quizViewVM.Grade = gradeName.GradeText;
            quizViewVM.Subject = subjectName.SubjectText;
            // save into database
            //var responce = await _QuizService.SaveQuizAsync(quizViewVM.Grade, quizViewVM.Subject, string.Empty,usr);

            return View(quizViewVM);
        }

        [HttpPost]
        public async Task<IActionResult> QuizResult(QuizViewVM model)
        {
            bool IsCorrect = false;
            int score = 0;
            foreach (var item in model.QuizMcqs)
            {
                if ( item.OptionsQnA.Answer.ToString() == item.OptionsQnA.Choice1.ToString() || item.OptionsQnA.AnswerL.ToString() == item.OptionsQnA.ChoiceL1.ToString())
                    IsCorrect = true;
                else if (item.OptionsQnA.Answer.ToString() == item.OptionsQnA.Choice2.ToString() || item.OptionsQnA.AnswerL.ToString() == item.OptionsQnA.ChoiceL2.ToString())
                    IsCorrect = true;
                else if (item.OptionsQnA.Answer.ToString() == item.OptionsQnA.Choice3.ToString() || item.OptionsQnA.AnswerL.ToString() == item.OptionsQnA.ChoiceL3.ToString())
                    IsCorrect = true;
                else if (item.OptionsQnA.Answer.ToString() == item.OptionsQnA.Choice4.ToString() || item.OptionsQnA.AnswerL.ToString() == item.OptionsQnA.ChoiceL4.ToString())
                    IsCorrect = true;
                else
                    IsCorrect = false;

                if (IsCorrect == true)
                    score += 1;

            }
            double pscore = score * 100 / model.TotalMarks;

            string quizresult = string.Empty;
            if (score <= model.TotalMarks && pscore >= 40.00)
            {
                quizresult = "Passed";
            }
            else
            {
                quizresult = "Failed";
            }

            Quiz obj = new Quiz()
            {

                QuizName = model.Subject + model.Grade + model.UserName + DateTime.Now,
                Grade = model.Grade,
                Subject = model.Subject,
                TotalScores = model.TotalMarks,
                ObtainedScores = score,
                Result = quizresult,
                Code = "Quiz",
                CreatedBy = _userManager.GetUserName(User),
                CreatedOn = DateTime.Now
            };
            await _QuizService.AddAsync(obj);
            return View(obj);

        }

        [ActionName("QuizResult")]
        public async Task<ActionResult> QuizResultAsync(int id)
        {
            var obj = await _QuizService.GetByIdAsync(id);
            return View(obj);
        }

        // JSON method for dropdownlists        code OK
        public JsonResult Subject(int id)
        {
            var sl = _context.Subjects.Where(s => s.GradeId == id).ToList();
            return new JsonResult(sl);
        }
        public JsonResult GetTableData(int id)
        {

            var query = _context.Topics.Where(s => s.SubjectId == id).ToList();

            var objtopiclist = new List<TopicsWithQCountsVM>();

            if (query.Any() == true)
            {
                foreach (var item in query)
                {
                    objtopiclist.Add(new TopicsWithQCountsVM()
                    {
                        Id = item.Id,
                        Code = item.Code,
                        TopicText = item.TopicText,

                    });
                }
            }
            return new JsonResult(objtopiclist);
        }


        public async Task<IActionResult> UserQuizAsync()
        {
            var user = _userManager.GetUserName(User);
            var obj = await _QuizService.GetAllQuizesByUser(user);
            return View(obj);

        }

        public IActionResult GetAllQuizes()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _QuizService.DeleteAsync(id);
            return RedirectToAction("UserQuiz", new {@id = User.Identity.Name});
        }

    }
}
