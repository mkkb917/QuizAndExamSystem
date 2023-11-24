using Microsoft.AspNetCore.Mvc;
using ExamSystem.Data.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Data.ViewModels;
using ExamSystem.Data.Static;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Filters;

namespace ExamSystem.Controllers
{
    [Authorize]
    [BreadcrumbActionFilter]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _service;
        private readonly Data.AppDbContext _context;

        public QuestionController(IQuestionService service, Data.AppDbContext context) //, /*DbContext context*/)
        {
            _service = service;
            _context = context;
        }
        // GET: Question
        public async Task<IActionResult> Index(int id)
        {
            var obj = new QuestionIndexVM();
            if (id == 0)
            {
                // display all the registered questions 
                obj.Questions = (List<Models.Question>?)await _service.GetAllAsync();
                return View(obj);
            }
            else
            {
                var TopicInfo = await _service.GetTopicById(id);
                var subject = await _service.GetSubjectById(TopicInfo.SubjectId);

                obj.TopicId = TopicInfo.Id;
                obj.TopicName = TopicInfo.TopicText;
                obj.SubjectId = TopicInfo.SubjectId;
                obj.SubjectName = subject.SubjectText;

                obj.Questions = await _service.GetAllQuestionsById(id);
            }
            return View(obj);
        }

        //public IActionResult UploadQuestionFile(int id)
        //{
        //    if (id != null)
        //    {
        //        TempData["TopicId"] = id;
        //    }
        //    else
        //    {
        //    }
        //}

        // JSON method for dropdownlists on Create method       code OK
        public JsonResult Subject(int id)
        {
            var sl = _context.Subjects.Where(s => s.GradeId == id).ToList();
            return new JsonResult(sl);
        }
        public JsonResult Topic(int id)
        {
            var tl = _context.Topics.Where(s => s.SubjectId == id).ToList();
            return new JsonResult(tl);
        }

        //GET: Question/Details/5
        public async Task<IActionResult> Details(int id)
        {
            //var SubjectText = await _service.GetByIdAsync(id);
            //TempData["SubjectText"] = SubjectText;
            var ObjQuestion = await _service.GetQuesDetailById(id);
            if (ObjQuestion == null) return View("NotFound");

            var choice = ObjQuestion.Choice.Answer;
            string answer = string.Empty;
            string answerL = string.Empty;
            if(choice == "ChoiceTitle1")
            {
                answer = ObjQuestion.Choice.Choice1;
                answerL = ObjQuestion.Choice.ChoiceL1;
            }
            else if (choice == "ChoiceTitle2")
            {
                answer = ObjQuestion.Choice.Choice2;
                answerL = ObjQuestion.Choice.ChoiceL2;
            }
            else if (choice == "ChoiceTitle3")
            {
                answer = ObjQuestion.Choice.Choice3;
                answerL = ObjQuestion.Choice.ChoiceL3;
            }
            else if (choice == "ChoiceTitle4")
            {
                answer = ObjQuestion.Choice.Choice4;
                answerL = ObjQuestion.Choice.ChoiceL4;
            }

            var responce = new QuestionVM()
            {
                // Quesiton Info
                Id = ObjQuestion.Id,
                Code = ObjQuestion.Code,
                QuestionText = ObjQuestion.QuestionText,
                QuestionTextL = ObjQuestion.QuestionTextL,
                CreatedBy = ObjQuestion.CreatedBy,
                CreatedOn = ObjQuestion.CreatedOn,
                UpdatedBy = ObjQuestion.UpdatedBy,
                UpdatedOn = ObjQuestion.UpdatedOn,
                DifficultyLevel = ObjQuestion.DifficultyLevel,
                Description = ObjQuestion.Description,
                Status = ObjQuestion.Status,
                // choice and answer info
                ChoiceTitle1 = ObjQuestion.Choice.Choice1,
                ChoiceTitle2 = ObjQuestion.Choice.Choice2,
                ChoiceTitle3 = ObjQuestion.Choice.Choice3,
                ChoiceTitle4 = ObjQuestion.Choice.Choice4,
                ChoiceTitleL1 = ObjQuestion.Choice.ChoiceL1,
                ChoiceTitleL2 = ObjQuestion.Choice.ChoiceL2,
                ChoiceTitleL3 = ObjQuestion.Choice.ChoiceL3,
                ChoiceTitleL4 = ObjQuestion.Choice.ChoiceL4,
                CorrectAnswer = answer,
                CorrectAnswerL = answerL,
                
            };
            return View(responce);
        }

        // GET: Question/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id != null)
            {
                TempData["TopicId"] = id;
            }
            else
            {
                var newQuestionDropdown = await _service.GetDDLObject();
                ViewBag.Grades = new SelectList(newQuestionDropdown.Grades, "Id", "GradeText");
                ViewBag.Subjects = new SelectList(newQuestionDropdown.Subjects, "Id", "SubjectText");
                ViewBag.Topics = new SelectList(newQuestionDropdown.Topics, "Id", "TopicText");
            }
            return View();
        }

        //POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionVM model, IFormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.QuestionType == QuestionTypes.MCQ)
                    {

                        int val = Convert.ToInt32(frm["option"]);
                        int valL = Convert.ToInt32(frm["optionl"]);

                        if (val >= 1 && val <= 4 && val == valL)
                        {
                            model.CorrectAnswer = model.GetType().GetProperty($"ChoiceTitle{val}").GetValue(model).ToString();
                            model.CorrectAnswerL = model.GetType().GetProperty($"ChoiceTitleL{val}").GetValue(model).ToString();
                        }
                    }
                    await _service.AddNewQuestion(model);
                    return RedirectToAction(nameof(Index), new { @id = model.TopicId });
                }
                var newQuestionDropdown = await _service.GetDDLObject();
                ViewBag.Grades = new SelectList(newQuestionDropdown.Grades, "Id", "GradeText");
                ViewBag.Subjects = new SelectList(newQuestionDropdown.Subjects, "Id", "SubjectText");
                ViewBag.Topics = new SelectList(newQuestionDropdown.Topics, "Id", "TopicText");
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return View(nameof(NotFound));
            }

        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ObjQuestion = await _service.GetQuesDetailById(id);
                if (ObjQuestion == null) return View("NotFound");

                var responce = new QuestionVM()
                {
                    // Quesiton Info
                    Id = ObjQuestion.Id,
                    Code = ObjQuestion.Code,
                    QuestionText = ObjQuestion.QuestionText,
                    QuestionTextL = ObjQuestion.QuestionTextL,
                    CreatedBy = ObjQuestion.CreatedBy,
                    CreatedOn = ObjQuestion.CreatedOn,
                    UpdatedBy = ObjQuestion.UpdatedBy,
                    UpdatedOn = ObjQuestion.UpdatedOn,
                    DifficultyLevel = ObjQuestion.DifficultyLevel,
                    Description = ObjQuestion.Description,
                    Status = ObjQuestion.Status,
                    // choice and answer info
                    ChoiceTitle1 = ObjQuestion.Choice.Choice1,
                    ChoiceTitle2 = ObjQuestion.Choice.Choice2,
                    ChoiceTitle3 = ObjQuestion.Choice.Choice3,
                    ChoiceTitle4 = ObjQuestion.Choice.Choice4,
                    ChoiceTitleL1 = ObjQuestion.Choice.ChoiceL1,
                    ChoiceTitleL2 = ObjQuestion.Choice.ChoiceL2,
                    ChoiceTitleL3 = ObjQuestion.Choice.ChoiceL3,
                    ChoiceTitleL4 = ObjQuestion.Choice.ChoiceL4,
                    CorrectAnswer = ObjQuestion.Choice.Answer,
                    CorrectAnswerL = ObjQuestion.Choice.AnswerL,
                    
                };
                return View(responce);
            }
            catch (Exception)
            {

                throw;
            }

        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  QuestionVM model, IFormCollection frm)
        {
            try
            {
                if(ModelState.IsValid && id == model.Id)
                {
                    if (model.QuestionType == QuestionTypes.MCQ)
                    {
                        model.CorrectAnswer = frm["option"].ToString();
                        model.CorrectAnswerL = frm["optionl"].ToString();
                    }
                    await _service.UpdateQuestion(id,model);
                    return RedirectToAction(nameof(Index), new { @id = model.TopicId });
                }

                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckForDuplicate(string InputStringValue)
        {
            // Check if an entry with the same text field value exists
            bool isDuplicate = await _service.SearchQuestionByString(InputStringValue);

            // Return a JSON response indicating whether it's a duplicate or not
            return Json(new { IsDuplicate = isDuplicate });
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var ObjQuestion = await _service.GetQuesDetailById(id);
            if (ObjQuestion == null) return View("NotFound");

            var responce = new QuestionVM()
            {
                // Quesiton Info
                Id = ObjQuestion.Id,
                Code = ObjQuestion.Code,
                QuestionText = ObjQuestion.QuestionText,
                QuestionTextL = ObjQuestion.QuestionTextL,
                CreatedBy = ObjQuestion.CreatedBy,
                CreatedOn = ObjQuestion.CreatedOn,
                UpdatedBy = ObjQuestion.UpdatedBy,
                UpdatedOn = ObjQuestion.UpdatedOn,
                DifficultyLevel = ObjQuestion.DifficultyLevel,
                Description = ObjQuestion.Description,
                Status = ObjQuestion.Status,
                // choice and answer info
                ChoiceTitle1 = ObjQuestion.Choice.Choice1,
                ChoiceTitle2 = ObjQuestion.Choice.Choice2,
                ChoiceTitle3 = ObjQuestion.Choice.Choice3,
                ChoiceTitle4 = ObjQuestion.Choice.Choice4,
                ChoiceTitleL1 = ObjQuestion.Choice.ChoiceL1,
                ChoiceTitleL2 = ObjQuestion.Choice.ChoiceL2,
                ChoiceTitleL3 = ObjQuestion.Choice.ChoiceL3,
                ChoiceTitleL4 = ObjQuestion.Choice.ChoiceL4,
                CorrectAnswer = ObjQuestion.Choice.Answer,
                CorrectAnswerL = ObjQuestion.Choice.AnswerL,

            };
            return View(responce);
        }

        //POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ObjQuestion = await _service.GetByIdAsync(id);
            if (ObjQuestion == null) return View("NotFound");
            await _service.DeleteQuestion(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
