using ExamSystem.Data.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Data.ViewModels;
using ExamSystem.Filters;
using ExamSystem.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly Data.AppDbContext _context;
        private readonly IQuestionService _QService;
        private readonly ISEDService _SedService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISubjectService _SubjectService;
        private readonly ILogger<DashboardController> _logger;
        private readonly IUploadsService _uploadsService;
        private readonly IQuestionService _questionService;
        private readonly IBookService _bookService;
        private readonly ITopicService _topicService;
        private readonly IQuizService _quizService;
        private readonly IPdfService _PdfService;


        public DashboardController( ILogger<DashboardController> logger,IUploadsService uploadsService,IQuestionService questionService, IBookService bookService,ITopicService topicService, IQuizService quizService, IPdfService PdfService, ISubjectService SubjectService, ISEDService SedService, Data.AppDbContext context, IQuestionService service, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _QService = service;
            _SedService = SedService;
            _SubjectService = SubjectService;
            _logger = logger;
            _uploadsService = uploadsService;
            _questionService = questionService;
            _bookService = bookService;
            _topicService = topicService;
            _quizService = quizService;
            _PdfService = PdfService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;


        }

        
        public async Task<IActionResult> Index()
        {
            var usr = new ApplicationUser()
            { UserName = _userManager.GetUserName(User) };
            var obj = new DashboardVM
            {
                Users  = await _context.Users.ToListAsync(),
                Grades = await _context.Grades.ToListAsync(),
                MyGrades = await _context.Grades.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Topics = await _context.Topics.ToListAsync(),
                MyTopics = await _context.Topics.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Subjects = await _context.Subjects.ToListAsync(),
                MySubjects = await _context.Subjects.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Questions = await _context.Questions.ToListAsync(),
                MyQuestions = await _context.Questions.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Sed = await _context.SED.ToListAsync(),
                MySed = await _context.SED.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Books = await _context.Books.ToListAsync(),
                MyBooks = await _context.Books.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Papers = await _context.GeneratedPapers.ToListAsync(),
                MyPapers = await _context.GeneratedPapers.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Quizes = await _context.Quizes.ToListAsync(),
                MyQuizes = await _context.Quizes.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
                Uploads = await _context.Uploads.ToListAsync(),
                MyUploads = await _context.Uploads.Where(u => u.CreatedBy == usr.UserName).ToListAsync(),
            };
            _logger.LogInformation("Index page of Corner Controller is accessed by {0}", User.Identity.Name);

            return View(obj);
        }







    }
}
