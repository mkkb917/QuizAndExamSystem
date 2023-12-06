using DinkToPdf;
using DinkToPdf.Contracts;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;
using QRCoder;
using System.Drawing;
using System.Linq;
//using Spire.Pdf.Graphics;


namespace ExamSystem.Data.Services
{
    public class PaperService : IPaperService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRazorPartialToStringRenderer _renderer;
        private readonly IConverter _pdfConverter;
        private readonly IQuestionService _service;
        private readonly ICompositeViewEngine _viewEngine;
        public PaperService(
            UserManager<ApplicationUser> userManager,
            AppDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IRazorPartialToStringRenderer renderer,
            IConverter pdfConverter,
            IQuestionService service,
            ICompositeViewEngine viewEngine)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _renderer = renderer;
            _service = service;
            _pdfConverter = pdfConverter;
            _viewEngine = viewEngine;
        }


        async Task<List<GeneratedPaper>> IPaperService.GetAllPapers()
        {
            var responce = await _context.GeneratedPapers.ToListAsync();
            return responce;
        }



        async Task<GeneratedPaper> IPaperService.GetPaperById(int id)
        {
            var responce = await _context.GeneratedPapers.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        async Task<List<GeneratedPaper>> IPaperService.GetAllPapersByUser(string Id)
        {
            var responce = await _context.GeneratedPapers.Where(n => n.CreatedBy == Id).ToListAsync();
            return responce;
        }

        public async Task<PaperViewVM> RenderPaper(int selectedClass, int selectedSubject, List<int>?selectedTopics, DateTime PaperDate, string? TeacherName, string? qrCode, ApplicationUser usr)
        {
            // get the current user question setting     
            var setting = new PaperSetting();
            var scheme = new PairingScheme();
            setting = await _context.PaperSettings.Where(u => u.UserName == usr.UserName).FirstOrDefaultAsync();
            if (setting != null)
            {
                setting.SchoolName = _context.SchoolInfos.Where(u => u.AppUser.UserName == usr.UserName).First().SchoolName;
                setting.SchoolLogo = _context.SchoolInfos.Where(u => u.AppUser.UserName == usr.UserName).First().SchoolLogo;
                setting.ClassName = await _context.Grades.Where(e => e.Id == selectedClass).Select(g => g.GradeText).SingleOrDefaultAsync();
                setting.SubjectName = await _context.Subjects.Where(e => e.Id == selectedSubject).Select(o => o.SubjectText).SingleOrDefaultAsync();
                setting.TeacherName = TeacherName;
                setting.QrCode = qrCode;
                if (PaperDate > DateTime.Today) { setting.ConductDate = PaperDate; } else { setting.ConductDate = DateTime.Today; }
            }

            // get the questions
            List<QnAs>? objQnA = new();
            List<QnAs>? seqQnA = new();
            List<QnAs>? LongQnA = new();
            List<Topic> topics = new();

            if (selectedTopics.Count==0)
            {
                topics = await _context.Topics.Where(t => t.SubjectId == selectedSubject && t.Status == Status.Active).ToListAsync();
            }
            else
            {
                topics = await _context.Topics.Where(t => t.SubjectId == selectedSubject && t.Status == Status.Active && selectedTopics.Contains(t.Id)).ToListAsync();
            }
            // traverse to each Unit for questions
            foreach (var Titem in topics)
            {
                List<Question> questions = new();
                if (selectedTopics.Count==0 || setting.PairingScheme == true)
                {
                    //set the total marks as per pairing scheme
                    setting.TotalMarks = Convert.ToInt32((Titem.MCQMarks * Titem.MCQCount) + (Titem.SEQMarks * Titem.SEQCount) + (Titem.LongQMarks * Titem.LongQCount));
                    setting.MCQsMarks = Titem.MCQMarks;
                    questions = await _context.Questions.Where(q =>q.Status==Status.Active && q.TopicId == Titem.Id && q.QuestionType == QuestionTypes.MCQ && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(Titem.MCQCount).ToListAsync();
                }
                else
                {
                    setting.TotalMarks = Convert.ToInt32((setting.MCQsMarks * setting.MCQsCount) + (setting.SEQsCount * setting.SEQsMarks) + (setting.LongQsMarks * setting.LongQsCount) + (setting.FillInBlanksMarks * setting.FillInBlanksCount));
                    int totalTopics = topics.Count;
                    int mcqCountPerTopic = setting.MCQsCount / totalTopics;
                    int remainingMCQs = setting.MCQsCount % totalTopics;
                    if (remainingMCQs > 0)
                    {
                        mcqCountPerTopic++;
                        remainingMCQs--;
                    }
                    questions = await _context.Questions.Where(q => q.Status == Status.Active && q.TopicId == Titem.Id && q.QuestionType == QuestionTypes.MCQ && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(mcqCountPerTopic).ToListAsync();
                }

                //SelectedPost = Answers.ElementAt(r.Next(0, Answers.Count()));
                // obejctive qustions
                foreach (var Qitem in questions)
                {

                    QnAs? varQnA = new();
                    varQnA.QuestionID = Qitem.Id;
                    varQnA.OptionsQnA = new Choice();
                    var ans = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    switch (setting.Medium)
                    {
                        case Medium.Urdu:
                            varQnA.QuestionTextL = Qitem.QuestionTextL;
                            varQnA.OptionsQnA.ChoiceL1 = ans.ChoiceL1;
                            varQnA.OptionsQnA.ChoiceL2 = ans.ChoiceL2;
                            varQnA.OptionsQnA.ChoiceL3 = ans.ChoiceL3;
                            varQnA.OptionsQnA.ChoiceL4 = ans.ChoiceL4;
                            varQnA.SelectedAnswerL = ans.AnswerL;
                            break;
                        case Medium.English:
                            varQnA.QuestionText = Qitem.QuestionText;
                            varQnA.OptionsQnA.Choice1 = ans.Choice1;
                            varQnA.OptionsQnA.Choice2 = ans.Choice2;
                            varQnA.OptionsQnA.Choice3 = ans.Choice3;
                            varQnA.OptionsQnA.Choice4 = ans.Choice4;
                            varQnA.SelectedAnswer = ans.Answer;
                            break;
                        default:
                            varQnA.QuestionTextL = Qitem.QuestionTextL;
                            varQnA.QuestionText = Qitem.QuestionText;
                            varQnA.OptionsQnA.ChoiceL1 = ans.ChoiceL1;
                            varQnA.OptionsQnA.ChoiceL2 = ans.ChoiceL2;
                            varQnA.OptionsQnA.ChoiceL3 = ans.ChoiceL3;
                            varQnA.OptionsQnA.ChoiceL4 = ans.ChoiceL4;
                            varQnA.OptionsQnA.Choice1 = ans.Choice1;
                            varQnA.OptionsQnA.Choice2 = ans.Choice2;
                            varQnA.OptionsQnA.Choice3 = ans.Choice3;
                            varQnA.OptionsQnA.Choice4 = ans.Choice4;
                            varQnA.SelectedAnswer = ans.Answer;
                            varQnA.SelectedAnswerL = ans.AnswerL;
                            break;
                    }
                    objQnA.Add(varQnA);
                }
                
            }

            // for Short Exam Questions
            foreach (var Sitem in topics)
            {
                List<Question> questions = new();
                if (selectedTopics.Count == 0 || setting.PairingScheme == true)
                {
                    setting.SEQsMarks = Sitem.SEQMarks;
                    questions = await _context.Questions.Where(q => q.Status == Status.Active && q.TopicId == Sitem.Id && q.QuestionType == QuestionTypes.SEQ && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(Sitem.SEQCount).ToListAsync();
                }
                else
                {
                    int totalTopics = topics.Count;
                    int SeqCountPerTopic = setting.SEQsCount / totalTopics;
                    int remainingQs = setting.SEQsCount % totalTopics;
                    if (remainingQs > 0)
                    {
                        SeqCountPerTopic++;
                        remainingQs--;
                    }
                    questions = await _context.Questions.Where(q => q.Status == Status.Active && q.TopicId == Sitem.Id && q.QuestionType == QuestionTypes.SEQ && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(SeqCountPerTopic).ToListAsync();
                }
                foreach (var Qitem in questions)
                {
                    QnAs? varQnA = new();
                    varQnA.QuestionID = Qitem.Id;
                    varQnA.OptionsQnA = new Choice();
                    var ans = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    switch (setting.Medium)
                    {
                        case Medium.Urdu:
                            varQnA.QuestionTextL = Qitem.QuestionTextL;
                            varQnA.SelectedAnswerL = ans.AnswerL;
                            break;
                        case Medium.English:
                            varQnA.QuestionText = Qitem.QuestionText;
                            varQnA.SelectedAnswer = ans.Answer;
                            break;
                        default:
                            varQnA.QuestionTextL = Qitem.QuestionTextL;
                            varQnA.QuestionText = Qitem.QuestionText;
                            varQnA.SelectedAnswer = ans.Answer;
                            varQnA.SelectedAnswerL = ans.AnswerL;
                            break;
                    }
                    seqQnA.Add(varQnA);
                }
            }

            // for Long Exam Questions
            foreach (var Litem in topics)
            {
                List<Question> questions = new();
                if (selectedTopics.Count == 0 || setting.PairingScheme == true)
                {
                    setting.LongQsMarks = Litem.LongQMarks;
                    questions = await _context.Questions.Where(q => q.Status == Status.Active && q.TopicId == Litem.Id && q.QuestionType == QuestionTypes.Long_Question && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(Litem.LongQCount).ToListAsync();
                }
                else
                {
                    int totalTopics = topics.Count;
                    int LongQCountPerTopic = setting.LongQsCount / totalTopics;
                    int remainingQs = setting.LongQsCount % totalTopics;
                    if (remainingQs > 0)
                    {
                        LongQCountPerTopic++;
                        remainingQs--;
                    }
                    questions = await _context.Questions.Where(q => q.Status == Status.Active && q.TopicId == Litem.Id && q.QuestionType == QuestionTypes.Long_Question && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(LongQCountPerTopic).ToListAsync();
                }
                foreach (var Qitem in questions)
                {
                    QnAs? varQnA = new();
                    varQnA.QuestionID = Qitem.Id; varQnA.OptionsQnA = new Choice();
                    var ans = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    switch (setting.Medium)
                    {
                        case Medium.Urdu:
                            varQnA.QuestionTextL = Qitem.QuestionTextL;
                            varQnA.SelectedAnswerL = ans.AnswerL;
                            break;
                        case Medium.English:
                            varQnA.QuestionText = Qitem.QuestionText;
                            varQnA.SelectedAnswer = ans.Answer;
                            break;
                        default:
                            varQnA.QuestionTextL = Qitem.QuestionTextL;
                            varQnA.QuestionText = Qitem.QuestionText;
                            varQnA.SelectedAnswer = ans.Answer;
                            varQnA.SelectedAnswerL = ans.AnswerL;
                            break;
                    }
                    LongQnA.Add(varQnA);
                }
            }
            PaperViewVM paperview = new PaperViewVM()
            {
                Setting = setting,
                MCQs = objQnA,
                SEQs = seqQnA,
                LongQs = LongQnA,

            };
            return paperview;
        }

        public Task<byte[]> RenderPdf(string paperView, string fileName)
        {
            var converter = new BasicConverter(new PdfTools());
            string webRootPath = _webHostEnvironment.WebRootPath;
            string PaperPath = webRootPath + WC.PaperPathPDF;
            
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                DPI = 300,
                ImageDPI = 900,
                Margins = new MarginSettings() { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                //Out = PaperPath + fileName,
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = paperView,
                UseExternalLinks = true,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = @"https://localhost:7278/wwwroot/css/paperStyle.css", LoadImages = true, Background = true },   //UserStyleSheet =  Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css")
                HeaderSettings = { FontName = "Arial", FontSize = 12, Line = true, Center = "Pdf Paper by ExamSystem", /*Right = barcode*/ },
                //HeaderSettings = { FontName = "Arial", FontSize = 12, Line = true, Center = "Pdf Paper by ExamSystem.com" },
                FooterSettings = { FontName = "Arial", FontSize = 12, Line = true, Center = "Generated on: ExamSystem", Right = "Page [page] of [toPage]" }
                //FooterSettings = { HtmUrl=FooterPath}
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            //byte[] file = _pdfConverter.Convert(pdf);
            var file = _pdfConverter.Convert(pdf);


            return Task.FromResult(file);

        }

        public string BarCodeGenerator(string QrCodeString, string guid)
        {
            // Qr Code image path
            string webRootPath = _webHostEnvironment.WebRootPath;
            string QrCodePath = webRootPath + WC.QrCodePath;

            //QrCode generator using QRCODER Library
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCodeString + guid, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(5);

            //save the byte array to image
            using (var ms = new MemoryStream(qrCodeAsBitmapByteArr))
            {
                Image qrCodeImage = Image.FromStream(ms);
                qrCodeImage.Save(QrCodePath + guid + ".png");
            }
            return (QrCodePath + guid + ".png");
        }


        async Task IPaperService.DeletePaper(int id)
        {

            // delete the choice table entry
            var Obj = await _context.GeneratedPapers.FirstOrDefaultAsync(n => n.Id == id);
            if (Obj != null)
            {
                // delete the record
                _context.Remove(Obj);
                await _context.SaveChangesAsync();

                // delete the uploaded file from directory
                string path = WC.PaperPathPDF;
                //delete the paper file
                var filePaper = Obj.PaperFile;
                DeleteFile(path, filePaper);
                //delete the Subjective paper file
                var fileSubPaper = Obj.PaperSubjetiveFile;
                DeleteFile(path, fileSubPaper);
                // delete the solutionfile
                var fileSolution = Obj.SolutionFile;
                DeleteFile(path, fileSolution);

                // delete the barcode file
                string BarCodepath = WC.QrCodePath;
                var fileBarCode = Obj.Barcode;
                DeleteFile(BarCodepath, fileBarCode);

            }
        }

        public void DeleteFile(string path, string Name)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileName = webRootPath + path + Name;
            // delete the file from directory
            if (fileName != null || fileName != string.Empty)
            {
                if (System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);
            }
        }

        public async Task<bool> SavePdfAsync(string gradeName, string subjectName, string fileObjectivePaper, string fileSubjectivePaper, string fileSolution, ApplicationUser user, string qrcode)
        {
            // save the file to database
            var obj = new GeneratedPaper()
            {
                CreatedBy = user.UserName,
                Grade = gradeName,
                Subject = subjectName,
                CreatedOn = DateTime.Now,
                PaperFile = fileObjectivePaper,
                PaperSubjetiveFile = fileSubjectivePaper,
                SolutionFile = fileSolution,
                Barcode = qrcode + ".png",
                IsAstive = true,
            };

            await _context.GeneratedPapers.AddAsync(obj);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaperSetting> GetPaperSettingByUser(ApplicationUser user)
        {
            var result= await _context.PaperSettings.Where(u => u.UserName == user.UserName).FirstOrDefaultAsync();
            return result;
        }

        public async Task<PaperSetting> GetPaperSettingById(int? Id)
        {
            var result = await _context.PaperSettings.FindAsync(Id);
            return result;
        }

        public async Task CreatePaperSetting(PaperSetting paperSetting)
        {
            await _context.PaperSettings.AddAsync(paperSetting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaperSetting(int Id, PaperSetting paperSetting)
        {
            // calculate the total marks
            var total = (paperSetting.MCQsMarks * paperSetting.MCQsCount) + (paperSetting.SEQsMarks * paperSetting.SEQsCount) + (paperSetting.LongQsCount * paperSetting.LongQsMarks) + (paperSetting.FillInBlanksMarks * paperSetting.FillInBlanksCount);
            //update record
            var obj = await _context.PaperSettings.FindAsync(Id);
            if (obj != null)
            {
                obj.UserName = paperSetting.UserName;
                obj.SchoolLogo = paperSetting.SchoolLogo;
                obj.SchoolName = paperSetting.SchoolName;
                obj.ExamName = paperSetting.ExamName;
                obj.TeacherName = paperSetting.TeacherName;
                obj.ClassName = paperSetting.ClassName;
                obj.SubjectName = paperSetting.SubjectName;
                obj.ConductDate = paperSetting.ConductDate;
                obj.Duration =  paperSetting.Duration;
                obj.DifficultyLevel = paperSetting.DifficultyLevel;
                obj.Medium = paperSetting.Medium;
                obj.QrCode = paperSetting.QrCode;
                obj.PairingScheme = paperSetting.PairingScheme;
                obj.Instructions = paperSetting.Instructions;
                obj.TotalMarks = paperSetting.TotalMarks;
                obj.PassingMarks = paperSetting.PassingMarks;
                obj.MCQsMarks = paperSetting.MCQsMarks;
                obj.SEQsMarks = paperSetting.SEQsMarks;
                obj.LongQsMarks = paperSetting.LongQsMarks;
                obj.FillInBlanksMarks = paperSetting.FillInBlanksMarks;
                obj.MCQsCount = paperSetting.MCQsCount;
                obj.SEQsCount = paperSetting.SEQsCount;
                obj.FillInBlanksCount = paperSetting.FillInBlanksCount;
                obj.LongQsCount = paperSetting.LongQsCount;
                obj.CreatedBy = paperSetting.CreatedBy;
                obj.CreatedOn = paperSetting.CreatedOn;
                obj.UpdatedBy = paperSetting.UpdatedBy;
                obj.UpdatedOn = paperSetting.UpdatedOn;
                obj.Status = paperSetting.Status;
            };

            //commet the changes to database
            await _context.SaveChangesAsync();
        }
    }



}
