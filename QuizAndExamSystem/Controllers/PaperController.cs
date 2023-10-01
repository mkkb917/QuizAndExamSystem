using ExamSystem.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Data.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Data.Static;
using ExamSystem.Filters;
using System;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class PaperController : Controller
    {
        private readonly IPdfService _pdfService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Data.AppDbContext _context;
        private readonly ISubjectService _SubjectService;
        private readonly IRazorPartialToStringRenderer _Renderer;

        public PaperController(IRazorPartialToStringRenderer Renderer, ISubjectService SubjectService, Data.AppDbContext context, IPdfService pdfService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _pdfService = pdfService;
            _userManager = userManager;
            _context = context;
            _SubjectService = SubjectService;
            _Renderer = Renderer;
        }

        public async Task<IActionResult> Index()
        {

            // list all the papers
            var Obj = await _pdfService.GetAllPapers();
            return View(Obj);
        }

        [HttpPost]
        public async Task<IActionResult> PaperView(string? GradeDDL, string? SubjectDDL, DateTime PaperDate, string? TeacherName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // get the class and subject DDL selected value
            int selectedClass = Convert.ToInt32(GradeDDL);
            int selectedSubject = Convert.ToInt32(SubjectDDL);
            //var selectedTopics = Convert.ToInt32(form["TopicsDDL"]);

            var grade = await _SubjectService.GetGradeById(selectedClass);
            string className = grade.GradeText;
            var subject = await _SubjectService.GetSubjectById(selectedSubject);
            string subjectName = subject.SubjectText;

            // get current user and school info 
            var usr = new ApplicationUser()
            { UserName = _userManager.GetUserName(User) };
            
            // call the function to render the questions bank
            var paperViewVM = await _pdfService.RenderPaper(selectedClass, selectedSubject, PaperDate, TeacherName, usr);
            
            if (paperViewVM.Setting == null)
            { return RedirectToAction("PaperSetting", new { @id = 0 }); }

            //create the Qr Code and pass to renderpaper method
            string guid = Guid.NewGuid().ToString().Substring(0, 5);
            string qrstring = className + subjectName;
            var qrcode = _pdfService.BarCodeGenerator(qrstring, guid);

            //inject barcode in the paperViewVM
            if (paperViewVM.Setting.QrCode == null)
            { paperViewVM.Setting.QrCode = qrcode.ToString(); }

           ;
            // get the html string for dinktopdf generator
            var paperObjectiveString = await _Renderer.RenderPartialToStringAsync("_ObjectiveBoardLayout", paperViewVM);
            var paperSubjectiveString = await _Renderer.RenderPartialToStringAsync("_SubjectiveBoardLayout", paperViewVM);
            var solString = await _Renderer.RenderPartialToStringAsync("_PaperSolutionView", paperViewVM);
        
            // create Unique file name for Objective paper, subjective paper, and solution
            string fileObjectiveName = "Objective "+paperViewVM.Setting.SubjectName + " " + guid + ".pdf";
            string fileSubjectiveName = "Subjective "+paperViewVM.Setting.SubjectName + " " + guid + ".pdf";
            string fileSolutionName = "Solution " + paperViewVM.Setting.SubjectName + " " + guid + ".pdf";
            
            // create the pdf paper  with BarCodes
            var FilePaper = await _pdfService.RenderPdf(paperObjectiveString, fileObjectiveName, qrcode);
            var FileSubjectivePaper = await _pdfService.RenderPdf(paperSubjectiveString, fileSubjectiveName, qrcode);
            var FileSolution = await _pdfService.RenderPdf(solString, fileSolutionName, qrcode);
            
            // save into database
            var responce = await _pdfService.SavePdfAsync(className, subjectName, fileObjectiveName, fileSubjectiveName, fileSolutionName, usr, guid);
            
            //if(responce.Equals(true))     total execution time is more than 3.63 minutes 
            return View(paperViewVM);
            //return RedirectToAction(nameof(UserPapers));
        }

        // JSON method for dropdownlists        code OK
        public JsonResult Subject(int id)
        {
            var sl = _context.Subjects.Where(s => s.GradeId == id && s.Status == Status.Active).ToList();
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
                        // manually query the total counts
                        McqCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.MCQ).Count(),
                        SeqCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.SEQ).Count(),
                        LongQCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.Long_Question).Count(),
                        FillinBlankCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.Fill_In_The_Blanks).Count()
                    });
                }
            }
            return new JsonResult(objtopiclist);
        }
        // GET: Pairing scheme dropdown fetch data
        public JsonResult GetPairingData(int id)
        {
            var responce = _context.Topics.Where(s => s.SubjectId == id).ToList();
            if (responce != null)
            {
                return new JsonResult(responce);
            }

            return new JsonResult(false);
        }

        [HttpGet]
        public async Task<IActionResult> PaperSetting()
        {
            var user = new ApplicationUser()
            {
                UserName = _userManager.GetUserName(User)
            };
            // fetch the record
            var result = await _context.PaperSettings.Where(u => u.UserName == user.UserName).FirstOrDefaultAsync();
            if (result != null)
            {
                PaperSetting objsetting = new PaperSetting()
                {
                    Id = result.Id,
                    UserName = result.UserName,
                    SchoolLogo = result.SchoolLogo,
                    SchoolName = result.SchoolName,
                    ExamName = result.ExamName,
                    TeacherName = result.TeacherName,
                    ClassName = result.ClassName,
                    SubjectName = result.SubjectName,
                    ConductDate = result.ConductDate,
                    Duration = result.Duration,
                    DifficultyLevel = result.DifficultyLevel,
                    Medium = result.Medium,
                    Instructions = result.Instructions,
                    TotalMarks = (result.MCQsMarks * result.MCQsCount) + (result.SEQsMarks * result.SEQsCount) + (result.LongQsCount * result.LongQsMarks) + (result.FillInBlanksMarks * result.FillInBlanksCount),
                    PassingMarks = result.PassingMarks,
                    MCQsMarks = result.MCQsMarks,
                    SEQsMarks = result.SEQsMarks,
                    LongQsMarks = result.LongQsMarks,
                    FillInBlanksMarks = result.FillInBlanksMarks,
                    MCQsCount = result.MCQsCount,
                    SEQsCount = result.SEQsCount,
                    FillInBlanksCount = result.FillInBlanksCount,
                    LongQsCount = result.LongQsCount,
                    CreatedBy = result.CreatedBy,
                    CreatedOn = result.CreatedOn,
                    UpdatedBy = result.UpdatedBy,
                    UpdatedOn = result.UpdatedOn,
                    Code = result.Code,

                    //Status = result.Status,
                };
                if (objsetting != null)
                {
                    return View(objsetting);
                }
                else
                {
                    return View(nameof(NotFound));
                }
            }
            else
            {
                //create new record by upsert method
                return RedirectToAction(nameof(Upsert), new { @id = 0 });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == 0)
            {
                var setting = new PaperSetting();
                return View(setting);
            }
            else if (id > 0)
            {
                var result = await _context.PaperSettings.FindAsync(id);
                if (result != null)
                {
                    PaperSetting setting = new()
                    {
                        Id = result.Id,
                        UserName = result.UserName,
                        SchoolLogo = result.SchoolLogo,
                        SchoolName = result.SchoolName,
                        ExamName = result.ExamName,
                        TeacherName = result.TeacherName,
                        ClassName = result.ClassName,
                        SubjectName = result.SubjectName,
                        ConductDate = result.ConductDate,
                        Duration = result.Duration,
                        DifficultyLevel = result.DifficultyLevel,
                        Medium = result.Medium,
                        Instructions = result.Instructions,
                        TotalMarks = (result.MCQsMarks * result.MCQsCount) + (result.SEQsMarks * result.SEQsCount) + (result.LongQsCount * result.LongQsMarks) + (result.FillInBlanksMarks * result.FillInBlanksCount),
                        PassingMarks = result.PassingMarks,
                        MCQsMarks = result.MCQsMarks,
                        SEQsMarks = result.SEQsMarks,
                        LongQsMarks = result.LongQsMarks,
                        FillInBlanksMarks = result.FillInBlanksMarks,
                        MCQsCount = result.MCQsCount,
                        SEQsCount = result.SEQsCount,
                        FillInBlanksCount = result.FillInBlanksCount,
                        LongQsCount = result.LongQsCount,
                        CreatedBy = result.CreatedBy,
                        CreatedOn = result.CreatedOn,
                        UpdatedBy = result.UpdatedBy,
                        UpdatedOn = result.UpdatedOn,
                        Status = result.Status,
                    };
                    return View(setting);
                }
            }
            else
            {
                return View("NotFound");
            }
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(int? id, PaperSetting model)
        {
            if (ModelState.IsValid)
            {

                if (id == 0)
                {
                    //Create new setting
                    PaperSetting setting = new()
                    {
                        UserName = model.UserName,
                        SchoolLogo = model.SchoolLogo,
                        SchoolName = model.SchoolName,
                        ExamName = model.ExamName,
                        TeacherName = model.TeacherName,
                        ClassName = model.ClassName,
                        SubjectName = model.SubjectName,
                        ConductDate = model.ConductDate,
                        Duration = model.Duration,
                        DifficultyLevel = model.DifficultyLevel,
                        Medium = model.Medium,
                        Instructions = model.Instructions,
                        TotalMarks = model.TotalMarks,
                        PassingMarks = model.PassingMarks,
                        MCQsMarks = model.MCQsMarks,
                        SEQsMarks = model.SEQsMarks,
                        LongQsMarks = model.LongQsMarks,
                        FillInBlanksMarks = model.FillInBlanksMarks,
                        MCQsCount = model.MCQsCount,
                        SEQsCount = model.SEQsCount,
                        FillInBlanksCount = model.FillInBlanksCount,
                        LongQsCount = model.LongQsCount,
                        Code = model.Code,
                        CreatedBy = model.CreatedBy,
                        CreatedOn = model.CreatedOn,
                        UpdatedBy = model.UpdatedBy,
                        UpdatedOn = model.UpdatedOn,
                    };
                    await _context.PaperSettings.AddAsync(setting);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(PaperSetting));
                }
                else
                {
                    // calculate the total marks
                    var total = (model.MCQsMarks * model.MCQsCount) + (model.SEQsMarks * model.SEQsCount) + (model.LongQsCount * model.LongQsMarks) + (model.FillInBlanksMarks * model.FillInBlanksCount);
                    //update record
                    var paperSetting = await _context.PaperSettings.FindAsync(id);
                    if (paperSetting != null)
                    {
                        paperSetting.UserName = model.UserName;
                        paperSetting.SchoolLogo = model.SchoolLogo;
                        paperSetting.SchoolName = model.SchoolName;
                        paperSetting.ExamName = model.ExamName;
                        paperSetting.TeacherName = model.TeacherName;
                        paperSetting.ClassName = model.ClassName;
                        paperSetting.SubjectName = model.SubjectName;
                        paperSetting.ConductDate = model.ConductDate;
                        paperSetting.Duration = model.Duration;
                        paperSetting.DifficultyLevel = model.DifficultyLevel;
                        paperSetting.Medium = model.Medium;
                        paperSetting.Instructions = model.Instructions;
                        paperSetting.TotalMarks = model.TotalMarks;
                        paperSetting.PassingMarks = model.PassingMarks;
                        paperSetting.MCQsMarks = model.MCQsMarks;
                        paperSetting.SEQsMarks = model.SEQsMarks;
                        paperSetting.LongQsMarks = model.LongQsMarks;
                        paperSetting.FillInBlanksMarks = model.FillInBlanksMarks;
                        paperSetting.MCQsCount = model.MCQsCount;
                        paperSetting.SEQsCount = model.SEQsCount;
                        paperSetting.FillInBlanksCount = model.FillInBlanksCount;
                        paperSetting.LongQsCount = model.LongQsCount;
                        paperSetting.CreatedBy = model.CreatedBy;
                        paperSetting.CreatedOn = model.CreatedOn;
                        paperSetting.UpdatedBy = model.UpdatedBy;
                        paperSetting.UpdatedOn = model.UpdatedOn;
                        paperSetting.Status = model.Status;
                    };

                    //commet the changes to database
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(PaperSetting));
                }
            }
            return RedirectToAction(nameof(PaperSetting), nameof(DashboardController));
        }

        //GET: PaperController/PairingScheme
        [HttpGet]
        public async Task<IActionResult> PairingScheme()
        {
            var newQuestionDropdown = await _context.Grades.OrderBy(t => t.GradeText).ToListAsync();
            ViewBag.Grades = new SelectList(newQuestionDropdown, "Id", "GradeText");
            return View();
        }

        // GET: PaperController/UserPapers
        public async Task<IActionResult> UserPapers()
        {
            var newQuestionDropdown = await _context.Grades.OrderBy(t => t.GradeText).ToListAsync();
            ViewBag.Grades = new SelectList(newQuestionDropdown, "Id", "GradeText");

            var user = _userManager.GetUserName(User);
            if (User.IsInRole(UserRoles.Admin))
            {
                // list all the papers
                var Obj = await _pdfService.GetAllPapers();
                return View(Obj);
            }
            else
            {
                var obj = await _pdfService.GetAllPapersByUser(user);
                return View(obj);
            }
        }

        // Download PAPER pdf from table
        public async Task<FileResult> GetObjectivePaper(int id)
        {
            var Obj = await _pdfService.GetPaperById(id);
            var paperName = Obj.PaperFile;
            var path = _webHostEnvironment.WebRootPath + WC.PaperPathPDF;
            // call the function to downlaod
            var memory = DownloadFile(paperName, path);
            return File(memory.ToArray(), "application/pdf", paperName);
        }
        // Download PAPER pdf from table
        public async Task<FileResult> GetSubjectivePaper(int id)
        {
            var Obj = await _pdfService.GetPaperById(id);
            var paperName = Obj.PaperSubjetiveFile;
            var path = _webHostEnvironment.WebRootPath + WC.PaperPathPDF;
            // call the function to downlaod
            var memory = DownloadFile(paperName, path);
            return File(memory.ToArray(), "application/pdf", paperName);
        }

        // Download Solution pdf from table
        public async Task<FileResult> GetSolution(int id)
        {
            var Obj = await _pdfService.GetPaperById(id);
            var solutionName = Obj.SolutionFile;
            var path = _webHostEnvironment.WebRootPath + WC.PaperPathPDF;
            // call the function to downlaod
            var memory = DownloadFile(solutionName, path);
            return File(memory.ToArray(), "application/pdf", solutionName);
        }
        private static MemoryStream DownloadFile(string filename, string uploadPath)
        {
            //var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var path = uploadPath + filename;
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var client = new System.Net.WebClient();
                var data =  client.DownloadData(path);
                memory = new MemoryStream(data);
            }
            memory.Position = 0;
            return memory;
        }

        // POST: PdfController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _pdfService.GetPaperById(id);
            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _pdfService.DeletePaper(id);
            return RedirectToAction("UserPapers", new { @id = User.Identity.Name });
        }
          


    }
}
