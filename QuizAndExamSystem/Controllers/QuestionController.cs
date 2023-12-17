using Microsoft.AspNetCore.Mvc;
using ExamSystem.Data.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Data.ViewModels;
using ExamSystem.Data.Static;
using Microsoft.AspNetCore.Authorization;
using ExamSystem.Filters;
using ExamSystem.Models;
using Microsoft.AspNetCore.JsonPatch.Adapters;

namespace ExamSystem.Controllers
{
    [Authorize]
    [BreadcrumbActionFilter]
    public class QuestionController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly ISubjectService _subjectService;
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _service;

        public QuestionController(ITopicService topicService,ISubjectService subjectService,ILogger<QuestionController> logger,IQuestionService service) 
        {
            _topicService = topicService;
            _subjectService = subjectService;
            _logger = logger;
            _service = service;
        }
        // GET: Question
        public async Task<IActionResult> Index(int id)
        {
            _logger.LogInformation("Index page of Question Controller is accessed by {0}", User.Identity.Name);
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
        
        // JSON method for dropdownlists on Create method       code OK
        public async Task<JsonResult> SubjectAsync(int id)
        {
            var sl = await _subjectService.GetAllActiveSubjectsById(id);
            _logger.LogInformation("Json Subjcts of Question Controller is accessed by {0}", User.Identity.Name);
            return new JsonResult(sl);
        }
        public async Task<JsonResult> TopicAsync(int id)
        {
            var tl = await _topicService.GetAllActiveTopicsById(id);
            _logger.LogInformation("Json Topics of Question Controller is accessed by {0}", User.Identity.Name);
            return new JsonResult(tl);
        }

        //GET: Question/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Details page of Question Controller is accessed by {0}", User.Identity.Name);
            
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
                QuestionType = ObjQuestion.QuestionType,
                TopicId = ObjQuestion.TopicId,
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
                // question meta info
                QuestionMetaVMs = ObjQuestion.QuestionMeta.Select(qm => new QuestionMetaVM
                {
                   BoardName = qm.BoardName,
                   ExamName = qm.ExamName,
                   ExamYear = qm.ExamYear,
                   MarkAs = qm.MarkAs,
                   Keywords = qm.Keywords,
                   Session = qm.Session,
                   
                }).ToList()

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
            _logger.LogInformation("Create page of Question Controller is accessed by {0}", User.Identity.Name);
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
                _logger.LogInformation("Create page of Question Controller is accessed by {0}", User.Identity.Name);
                _logger.LogInformation("The new Record is successfully created by {0}", User.Identity.Name);
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
                _logger.LogInformation("Edit page of Question Controller is accessed by {0}", User.Identity.Name);
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
                _logger.LogInformation("Edit page of Question Controller is accessed by {0}", User.Identity.Name);
                if (ModelState.IsValid && id == model.Id)
                {
                    //model.CorrectAnswer
                    int val = Convert.ToInt32(frm["option"]);
                    int valL = Convert.ToInt32(frm["optionl"]);

                    if (val >= 1 && val <= 4 && val == valL)
                    {
                        model.CorrectAnswer = model.GetType().GetProperty($"ChoiceTitle{val}").GetValue(model).ToString();
                        model.CorrectAnswerL = model.GetType().GetProperty($"ChoiceTitleL{val}").GetValue(model).ToString();
                    }
                    await _service.UpdateQuestion(id,model);
                    _logger.LogInformation("The record is successfully modified by {0}", User.Identity.Name);
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


        //Get: Question/approve
        public async Task<IActionResult> Approve()
        {

            Status _status = Status.Pending;
            var obj = await _service.GetAllQuestionsByStatus(_status);
            var responce = new QuestionIndexVM()
            {

                Questions = obj,
            };
            _logger.LogInformation("Approve page of Question Controller is accessed by {0}", User.Identity.Name);
            return View(responce);
        }

        #region QuestionMeta Section
        /// <summary>
        /// meta section starts 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //Add new question meta 
        [HttpGet]
        public IActionResult CreateQuestionMeta(int id)
        {
            var obj = new QuestionMetaVM();
            if (id != 0)
            {
                obj.QuestionId = id;
            }
            
            _logger.LogInformation("Create page of QuestionMeta Information is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionMeta(QuestionMetaVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = new QuestionMeta()
                    {
                        QuestionId = model.QuestionId,
                        BoardName = model.BoardName,
                        Session = model.Session,
                        ExamName = model.ExamName,
                        ExamYear = model.ExamYear,
                        Keywords = model.Keywords,
                        MarkAs = model.MarkAs,
                        Description = model.Description,
                    };
                    await _service.AddNewQuestionMeta(obj);
                    _logger.LogInformation("New question meta record is created by {0}", User.Identity.Name);
                    return RedirectToAction(nameof(Details), new { @id = model.QuestionId });
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //edit the question meta
        [HttpGet]
        public async Task<IActionResult> EditQuestionMeta(int id)
        {
            var obj = new QuestionMetaVM();
            if (id != 0)
            {
                var responce = await _service.GetQuestionMetaById(id);
                if (responce != null)
                {
                    obj.BoardName = responce.BoardName;
                    obj.Session = responce.Session;
                    obj.Keywords = responce.Keywords;
                    obj.ExamName = responce.ExamName;
                    obj.ExamYear = responce.ExamYear;
                    obj.MarkAs = responce.MarkAs;
                    obj.Description = responce.Description;
                    obj.QuestionId = id;

                    _logger.LogInformation("edit of metaQuestion is requested by {0}", User.Identity.Name);
                    return View(obj);
                }
                else
                {
                    return RedirectToAction(nameof(CreateQuestionMeta));
                }
            }
            return RedirectToAction(nameof(NotFound));
        }

        //POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMeta(int id)
        {
            var Obj = await _service.GetQuestionMetaById(id);
            if (Obj == null) return View("NotFound");
            await _service.DeleteQuestionMeta(id);
            _logger.LogInformation("Delete button of Question Meta is accessed by {0}", User.Identity.Name);
            _logger.LogInformation("Record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction(nameof(Details), new { @id = id });
        }

        #endregion

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
            _logger.LogInformation("Delete page of Question Controller is accessed by {0}", User.Identity.Name);
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
            _logger.LogInformation("Delete page of Question Controller is accessed by {0}", User.Identity.Name);
            _logger.LogInformation("Record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
