using ExamSystem.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Data.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Data.Static;
using ExamSystem.Filters;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class PaperController : Controller
    {
        private readonly IPdfService _pdfService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PaperController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Data.AppDbContext _context;
        private readonly ISubjectService _SubjectService;
        private readonly IRazorPartialToStringRenderer _Renderer;

        public PaperController(IRazorPartialToStringRenderer Renderer,
                                ISubjectService SubjectService,
                                Data.AppDbContext context,
                                IPdfService pdfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                ILogger<PaperController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _pdfService = pdfService;
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _SubjectService = SubjectService;
            _Renderer = Renderer;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("User visited index page of Page controller");
            // list all the papers
            var Obj = await _pdfService.GetAllPapers();
            return View(Obj);
        }
        

        // JSON method for dropdownlists        code OK
        public JsonResult Subject(int id)
        {
            var sl = _context.Subjects.Where(s => s.GradeId == id && s.Status == Status.Active).ToList();
            return new JsonResult(sl);
        }

        public JsonResult GetTopicsData(int id)
        {

            var query = _context.Topics.Where(s => s.SubjectId == id && s.Status == Status.Active).ToList();

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
                        McqCount = item.MCQCount,
                        SeqCount = item.SEQCount,
                        LongQCount = item.LongQCount,
                        FillinBlankCount = 0,
                        //manually query the total counts
                        //McqCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.MCQ).Count(),
                        //SeqCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.SEQ).Count(),
                        //LongQCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.Long_Question).Count(),
                        //FillinBlankCount = _context.Questions.Where(q => q.TopicId == item.Id && q.QuestionType == QuestionTypes.Fill_In_The_Blanks).Count()
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



        [HttpPost]
        public async Task<IActionResult> SavePairingData([FromBody] List<Dictionary<string, string>> entities)
        {
            try
            {
                // data sample of entity
                //[{0: "1005", 1: "Problem Solving", 2: "2", 3: "1", 4: "2", 5: "0", 6: "3", 7: "5"},…]
                int defaultValue = 0;
                if (entities == null) return StatusCode(500, new { status = "Error", message = "the data array is empty." });

                foreach (var entity in entities)
                {
                    var obj = new TopicsWithQCountsVM();
                    // Loop through each dictionary
                    foreach (var kvp in entity)
                    {
                        // Extract values based on the index and assign them to respective variables
                        if (kvp.Key == "0")
                        {
                            obj.Id = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                        else if (kvp.Key == "1")
                        {
                            obj.TopicText = kvp.Value;
                        }
                        else if (kvp.Key == "2")
                        {
                            obj.McqMarks = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                        else if (kvp.Key == "3")
                        {
                            obj.McqCount = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                        else if (kvp.Key == "4")
                        {
                            obj.SeqMarks = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                        else if (kvp.Key == "5")
                        {
                            obj.SeqCount = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                        else if (kvp.Key == "6")
                        {
                            obj.LongQMarks = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                        else if (kvp.Key == "7")
                        {
                            obj.LongQCount = int.TryParse(kvp.Value, out int parsedValue) ? parsedValue : defaultValue;
                        }
                    }
                    //save each iteration data to database 
                    var objscheme = await _context.Topics.FirstOrDefaultAsync(s => s.Id == obj.Id);
                    if (objscheme != null)
                    {
                        objscheme.MCQMarks = obj.McqMarks;
                        objscheme.MCQCount = obj.McqCount;
                        objscheme.SEQMarks = obj.SeqMarks;
                        objscheme.MCQCount = obj.McqCount;
                        objscheme.LongQMarks = obj.LongQMarks;
                        objscheme.LongQCount = obj.LongQCount;
                    }
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("{0} saved his Pairing Scheme on date:{1}", User.Identity.Name );
                }


                return Ok(new { status = "Success", message = "Data saved successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, new { status = "Error", message = "An error occurred while saving the data.", ex.Message });
            }
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
                    PairingScheme = result.PairingScheme,
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
                _logger.LogInformation("{0} request to access his paper settting ", User.Identity.Name );

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
                        QrCode = model.QrCode,
                        PairingScheme = model.PairingScheme,
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
                        paperSetting.QrCode = model.QrCode;
                        paperSetting.PairingScheme = model.PairingScheme;
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
                    _logger.LogInformation("{0} Updated his Pairing Scheme", User.Identity.Name );
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
            _logger.LogInformation("{0} requested to access the pairing scheme ", User.Identity.Name );
            return View();
        }

        // GET: PaperController/UserPapers
        public async Task<IActionResult> UserPapers()
        {
            var newQuestionDropdown = await _context.Grades.OrderBy(t => t.GradeText).ToListAsync();
            ViewBag.Grades = new SelectList(newQuestionDropdown, "Id", "GradeText");

            var user = _userManager.GetUserName(User);
            _logger.LogInformation("{0} accessed his their generated papers list ", User.Identity.Name );

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

        [HttpPost]
        public async Task<IActionResult> GeneratePaper(string? GradeDDL, string? SubjectDDL, DateTime PaperDate, string? TeacherName, bool Advance, List<string>? topicList)
        {
            _logger.LogInformation("Generate Paper functioned called by {0}",User.Identity.Name);
            // get the class and subject DDL selected value
            int selectedClass = Convert.ToInt32(GradeDDL);
            int selectedSubject = Convert.ToInt32(SubjectDDL);
            List<int>? selectedTopics = new();

            var grade = await _SubjectService.GetGradeById(selectedClass);
            string className = grade.GradeText.ToString();
            var subject = await _SubjectService.GetSubjectById(selectedSubject);
            string subjectName = subject.SubjectText.ToString();

            // get current user and school info 
            var usr = new ApplicationUser()
            { UserName = _userManager.GetUserName(User) };

            //create the Qr Code and pass to renderpaper method
            string guid = Guid.NewGuid().ToString().Substring(0, 5);
            string qrstring = className + subjectName;
            var qrcode = _pdfService.BarCodeGenerator(qrstring, guid);
            _logger.LogInformation("Barcode generated by calling BarCodeGenerator function");
            var paperViewVM = new PaperViewVM();
            if (Advance == true) // its means user want to create small test of chapter wise
            {
                if (topicList != null)
                {
                    // conver the string list of topics to int list
                    selectedTopics = topicList.Select(int.Parse).ToList();
                }
            }
            paperViewVM = await _pdfService.RenderPaper(selectedClass, selectedSubject, selectedTopics, PaperDate, TeacherName, qrcode, usr);
            _logger.LogInformation("Render paper is called and paper onject is generated");

            if (paperViewVM.Setting == null)
            { return RedirectToAction("PaperSetting", new { @id = 0 }); }

            //inject barcode in the paperViewVM
            //if (paperViewVM.Setting.QrCode == null)
            //{ paperViewVM.Setting.QrCode = qrcode.ToString(); }

            // get the html string for dinktopdf generator
            var paperObjectiveString = await _Renderer.RenderPartialToStringAsync("_ObjectiveBoardLayout", paperViewVM);
            var paperSubjectiveString = await _Renderer.RenderPartialToStringAsync("_SubjectiveBoardLayout", paperViewVM);
            var solString = await _Renderer.RenderPartialToStringAsync("_PaperSolutionView", paperViewVM);
            _logger.LogInformation("RenderPartial function is called and paper object is converted to string");
            //// create Unique file name for Objective paper, subjective paper, and solution
            //string fileObjectiveName = "Objective " + paperViewVM.Setting.SubjectName + " " + guid + ".pdf";
            //string fileSubjectiveName = "Subjective " + paperViewVM.Setting.SubjectName + " " + guid + ".pdf";
            //string fileSolutionName = "Solution " + paperViewVM.Setting.SubjectName + " " + guid + ".pdf";

            //// create the pdf paper  with BarCodes
            //var FilePaper = await _pdfService.RenderPdf(paperObjectiveString, fileObjectiveName, qrcode);
            //var FileSubjectivePaper = await _pdfService.RenderPdf(paperSubjectiveString, fileSubjectiveName, qrcode);
            //var FileSolution = await _pdfService.RenderPdf(solString, fileSolutionName, qrcode);

            // save into database
            var responce = await _pdfService.SavePdfAsync(className, subjectName, paperObjectiveString, paperSubjectiveString, solString, usr, guid);
            //var responce = await _pdfService.SavePdfAsync(className, subjectName, fileObjectiveName, fileSubjectiveName, fileSolutionName, usr, guid);
            _logger.LogInformation("Paper string is saved into database for {0} at {1}", User.Identity.Name,DateTime.Now.Date);
            //if(responce.Equals(true))     total execution time is more than 3.63 minutes 
            //return View(paperViewVM);
            return RedirectToAction(nameof(UserPapers));
        }

        // Download PAPER pdf from table
        public async Task<IActionResult> PaperView(int id, string type)
        {
            var Obj = await _pdfService.GetPaperById(id);

            //get the barcode file name and address of file
            var QrCodePath = _webHostEnvironment.WebRootPath + WC.QrCodePath;
            //var qrCodeName = Path.GetFileName(QrCodePath + Obj.Barcode);

            var fileName = Obj.Subject + " " + Path.GetFileNameWithoutExtension(QrCodePath + Obj.Barcode);

            string? paperObj;
            //var paperFile;

            if (type == "objective")
            {
                paperObj = Obj.PaperFile;
                var paperFile = await _pdfService.RenderPdf(paperObj, fileName);
                Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
                _logger.LogInformation("{0} downloaded/View the objective paper {1} ",User.Identity.Name,fileName,DateTime.Now.Date);
                return new FileContentResult(paperFile, "application/pdf");
            }
            else if (type == "subjective")
            {
                paperObj = Obj.PaperSubjetiveFile;
                var paperFile = await _pdfService.RenderPdf(paperObj, fileName);
                Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
                _logger.LogInformation("{0} downloaded/View the subjective paper {1}  ", User.Identity.Name, fileName );
                return new FileContentResult(paperFile, "application/pdf");
            }
            else if (type == "solution")
            {
                paperObj = Obj.SolutionFile;
                var paperFile = await _pdfService.RenderPdf(paperObj, fileName);
                Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
                _logger.LogInformation("{0} downloaded/View the solution paper {1}  ", User.Identity.Name, fileName );
                return new FileContentResult(paperFile, "application/pdf");
            }

            //Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");


            return View();
        }


        // Download PAPER pdf from table
        //public async Task<FileResult> GetSubjectivePaper(int id)
        //{
        //    var Obj = await _pdfService.GetPaperById(id);
        //    var paperName = Obj.PaperSubjetiveFile;
        //    var path = _webHostEnvironment.WebRootPath + WC.PaperPathPDF;
        //    // call the function to downlaod
        //    //var memory = DownloadFile(paperName, path);
        //    byte[] memory = DownloadFile(paperName);
        //    return File(memory.ToArray(), "application/pdf", paperName);
        //}

        // Download Solution pdf from table
        //public async Task<FileResult> GetSolution(int id)
        //{
        //    var Obj = await _pdfService.GetPaperById(id);
        //    var solutionName = Obj.SolutionFile;
        //    var path = _webHostEnvironment.WebRootPath + WC.PaperPathPDF;
        //    // call the function to downlaod
        //    //var memory = DownloadFile(solutionName, path);
        //    var memory;// = DownloadFile(solutionName);
        //    return File(memory.ToArray(), "application/pdf", solutionName);
        //}
        //private static MemoryStream DownloadFile(Task<byte[]> filePaper)//, string uploadPath)
        //{
        //    //var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
        //    //var path = uploadPath + filePaper;
        //    var memory = new MemoryStream();
        //    //if (System.IO.File.Exists(path))
        //    //{
        //    var client = new System.Net.WebClient();
        //    var data = client.DownloadData(filePaper);
        //    memory = new MemoryStream(data);

        //    memory.Position = 0;
        //    return memory;
        //}

        // POST: PdfController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _pdfService.GetPaperById(id);
            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _pdfService.DeletePaper(id);
            _logger.LogInformation("{0} deleted the paper with id:{1}", User.Identity.Name,Obj.Id );
            return RedirectToAction("UserPapers", new { @id = User.Identity.Name });
        }

        

    }
}
