using DinkToPdf;
using DinkToPdf.Contracts;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ExamSystem.Models;
using System.Net;
using QRCoder;
using System.Drawing;

namespace ExamSystem.Data.Services
{
    public class PdfService : IPdfService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRazorPartialToStringRenderer _renderer;
        private readonly IConverter _pdfConverter;
        private readonly IQuestionService _service;
        private readonly ICompositeViewEngine _viewEngine;
        public PdfService(
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


        async Task<List<GeneratedPaper>> IPdfService.GetAllPapers()
        {
            var responce = await _context.GeneratedPapers.ToListAsync();
            return responce;
        }



        async Task<GeneratedPaper> IPdfService.GetPaperById(int id)
        {
            var responce = await _context.GeneratedPapers.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        async Task<List<GeneratedPaper>> IPdfService.GetAllPapersByUser(string Id)
        {
            var responce = await _context.GeneratedPapers.Where(n => n.CreatedBy == Id).ToListAsync();
            return responce;
        }

        public async Task<PaperViewVM> RenderPaper(int selectedClass, int selectedSubject, DateTime PaperDate, string? TeacherName, ApplicationUser usr)
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
                setting.TotalMarks = Convert.ToInt32((setting.MCQsMarks * setting.MCQsCount) + (setting.SEQsCount * setting.SEQsMarks) + (setting.LongQsMarks * setting.LongQsCount) + (setting.FillInBlanksMarks * setting.FillInBlanksCount));
                setting.TeacherName = TeacherName;
                if (PaperDate > DateTime.Today) { setting.ConductDate = PaperDate; } else { setting.ConductDate = DateTime.Today; }
            }
           
            // get the questions
            List<QnAs>? objQnA = new();
            List<QnAs>? seqQnA = new();
            List<QnAs>? LongQnA = new();

            //random number 
            var r = new Random();

            int mcqCount = Convert.ToInt32(setting.MCQsCount);
            if (mcqCount == 0)
                mcqCount = 10; 

            var topics = await _context.Topics.Where(t => t.SubjectId == selectedSubject).ToListAsync();
            // traverse to each Unit for questions
            foreach (var Titem in topics)
            {
                //SelectedPost = Answers.ElementAt(r.Next(0, Answers.Count()));
                // obejctive qustions
                var questions = await _context.Questions.Where(q => q.TopicId == Titem.Id && q.QuestionType == QuestionTypes.MCQ && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(Titem.MCQCount).ToListAsync();
                foreach (var Qitem in questions)
                {
                    QnAs? varQnA = new();
                    varQnA.QuestionID = Qitem.Id;
                    varQnA.QuestionTextL = Qitem.QuestionTextL;
                    varQnA.QuestionText = Qitem.QuestionText;
                    varQnA.QuestionType = Qitem.QuestionType;
                    //var options = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).Select(o => new { OptionID = o.Id, Option1 = o.Choice1, OptionL1 = o.ChoiceL1, Option2 = o.Choice2, OptionL2 = o.ChoiceL2, Option3 = o.Choice3, OptionL3 = o.ChoiceL3, Option4 = o.Choice4, OptionL4 = o.ChoiceL4, Answer = o.Answer }).ToListAsync();
                    varQnA.OptionsQnA = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    objQnA.Add(varQnA);
                }
            }

            int seqCount = Convert.ToInt32(setting.SEQsCount);
            if (seqCount == 0)
            { seqCount = 5; }
            // for Short Exam Questions
            foreach (var Sitem in topics)
            {
                // obejctive qustions
                var questions = await _context.Questions.Where(q => q.TopicId == Sitem.Id && q.QuestionType == QuestionTypes.SEQ && q.DifficultyLevel==setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(Sitem.SEQCount).ToListAsync();
                foreach (var Qitem in questions)
                {
                    QnAs? varQnA = new();
                    varQnA.QuestionID = Qitem.Id;
                    varQnA.QuestionTextL = Qitem.QuestionTextL;
                    varQnA.QuestionText = Qitem.QuestionText;
                    varQnA.QuestionType = Qitem.QuestionType;
                    varQnA.OptionsQnA = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
                    seqQnA.Add(varQnA);
                }
            }

            int leqCount = Convert.ToInt32(setting.LongQsCount);
            if (leqCount == 0)
            { leqCount = 5; }
            // for Long Exam Questions
            foreach (var Litem in topics)
            {
                
                // obejctive qustions
                var questions = await _context.Questions.Where(q => q.TopicId == Litem.Id && q.QuestionType == QuestionTypes.Long_Question && q.DifficultyLevel == setting.DifficultyLevel).OrderBy(o => Guid.NewGuid()).Take(Litem.LongQCount).ToListAsync();
                foreach (var Qitem in questions)
                {
                    QnAs? varQnA = new();

                    varQnA.QuestionID = Qitem.Id;
                    varQnA.QuestionTextL = Qitem.QuestionTextL;
                    varQnA.QuestionText = Qitem.QuestionText;
                    varQnA.QuestionType = Qitem.QuestionType;

                    //var options = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).Select(o => new { OptionID = o.Id, Option1 = o.Choice1, OptionL1 = o.ChoiceL1, Option2 = o.Choice2, OptionL2 = o.ChoiceL2, Option3 = o.Choice3, OptionL3 = o.ChoiceL3, Option4 = o.Choice4, OptionL4 = o.ChoiceL4, Answer = o.Answer }).ToListAsync();
                    varQnA.OptionsQnA = await _context.Choices.Where(q => q.QuestionId == Qitem.Id).FirstOrDefaultAsync();
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

        public Task<byte[]> RenderPdf(string paperView, string fileName, string qrcode)
        {
            var converter = new BasicConverter(new PdfTools());
            string webRootPath = _webHostEnvironment.WebRootPath;
            string PaperPath = webRootPath + WC.PaperPathPDF;

            var htmlDoc = new StringBuilder();
            htmlDoc.AppendLine("<td>");
            //It fetches under wwwroot and include the current generating code image
            var path = _webHostEnvironment.WebRootPath + WC.QrCodePath + qrcode;
            htmlDoc.AppendLine($"<img style=\"margin-bottom:0px;\" src=\"{path}\" />");
            htmlDoc.AppendLine("</td>");
            var barcode = $"<img style=\"margin-bottom:0px;\" src=\"~{path}\"/>";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                DPI = 300,
                ImageDPI = 900,
                Margins = new MarginSettings() { Top = 10, Bottom = 5, Left = 5, Right = 5 },
                Out = PaperPath + fileName,
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlDoc + paperView,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = @"https://localhost:7278/wwwroot/css/paperStyle.css", LoadImages = true, Background = true },   //UserStyleSheet =  Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css")
                HeaderSettings = { FontName = "Arial", FontSize = 12, Line = true, Center = "Pdf Paper by ExamSystem.com" , Right = barcode },
                FooterSettings = { FontName = "Arial", FontSize = 12, Line = true, Left = "Generated on: www.examsystem.com", Right = "Page [page] of [toPage]" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] file = _pdfConverter.Convert(pdf);
            //byte[] file = converter.Convert(pdf);
            

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
            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);

            //save the byte array to image
            using (var ms = new MemoryStream(qrCodeAsBitmapByteArr))
            {
                Image qrCodeImage = Image.FromStream(ms);
                qrCodeImage.Save(QrCodePath + guid + ".png");
            }
            return (QrCodeString + ".png");
        }

       
        async Task IPdfService.DeletePaper(int id)
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
                Barcode = qrcode + ".jpeg",
                IsAstive = true,
            };

            await _context.GeneratedPapers.AddAsync(obj);
            await _context.SaveChangesAsync();
            return true;
        }

    }



}
