using ExamSystem.Data;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using ExamSystem.Filters;
using ExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class TopicController : Controller
    {
        private readonly ITopicService _service;
        private readonly ILogger<TopicController> _logger;

        public TopicController(ITopicService service, ILogger<TopicController> logger) //, AppDbContext context)
        {
            _service = service;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int Id)
        {
            var obj = new List<TopicsVM>();
            obj = (List<TopicsVM>)await _service.GetAllAsync();
            foreach (var item in obj)
            {
                item.Questions = await _service.GetAllQuestionsById(item.Id);
            }
            _logger.LogInformation("Index page of Topic Contorller is accessed by {0}", User.Identity.Name);
            return View(obj);

        }

        // topics list on subject id
        //Get:  topic/topiclist/1
        public async Task<IActionResult> TopicsList(int id)
        {
            var subject = await _service.GetSubjectById(id);
            var obj = new TopicIndexVM();
            obj.Topic = await _service.GetAllTopicsById(id);
            obj.SubjectName = subject.SubjectText;
            obj.SubjectId = subject.Id;
            obj.GradeId = subject.GradeId;
            foreach (var item in obj.Topic)
            {
                item.Question = await _service.GetAllQuestionsById(item.Id);
                item.SubjectId = subject.Id;
            }
            _logger.LogInformation("Topics list page of Topic Contorller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }



        // Get: Topic/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if (id != null)
            {
                TempData["SubjectId"] = id;
            }
            else
            {
                var SubjectListData = await _service.GetSubjectList();
                ViewBag.Subject = new SelectList(SubjectListData.Subjects, "Id", "SubjectText");
            }
            _logger.LogInformation("Create page of Topic Contorller is accessed by {0}", User.Identity.Name);
            return View();
        }


        //Post: Topic/Create            create a new entry into database
        [HttpPost]
        public async Task<IActionResult> Create(TopicsVM topic)
        {
            if (!ModelState.IsValid)
            {
                //var SubjectListData = await _service.GetGradeList();
                //ViewBag.Grades = new SelectList(SubjectListData.Grades, "Id", "GradeText");
                //ViewBag.Subjects = new SelectList(SubjectListData.Subjects, "Id", "SubjectText");
                return View(topic);
            }
            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count == 0)
            {
                TempData["error"] = "Image not attached";
                return View(topic);
            }
            // pass the file name, path and file to uploader

            string fileExtension = Path.GetExtension(files[0].FileName);
            //check the extension for image files only
            if (!fileExtension.Equals(".jpeg") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".png"))
            {
                TempData["error"] = "Invlaid Image format";
                return View(topic);
            }
            // pass the files object to funtion to save file and create thumbnail in directory
            var uploadImage = _service.DeleteOldAndUploadNewFile(files, null);
            topic.Image = uploadImage;
            await _service.AddNewTopic(topic);
            _logger.LogInformation("New record is successfully created by {0}", User.Identity.Name);
            return RedirectToAction(nameof(TopicsList), new { @id = topic.SubjectId });

        }

        //edit a Topic
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var Obj = await _service.GetByIdAsync(id);
            if (Obj == null) return View("NotFound");
            var responce = new TopicsVM()
            {
                Id = Obj.Id,
                Code = Obj.Code,
                TopicText = Obj.TopicText,
                Image = Obj.Image,
                Status = Obj.Status,
                SubjectId = Obj.SubjectId,
                MCQCount = Obj.MCQCount,
                MCQMarks = Obj.MCQMarks,
                SEQCount = Obj.SEQCount,
                SEQMarks = Obj.SEQMarks,
                LongQCount = Obj.LongQCount,
                LongQMarks = Obj.LongQMarks,
                Description = Obj.Description,
                CreatedOn = Obj.CreatedOn,
                CreatedBy = Obj.CreatedBy,
                UpdatedOn = Obj.UpdatedOn,
                UpdatedBy = Obj.UpdatedBy
            };
            _logger.LogInformation("Edit page of Topic Contorller is accessed by {0}", User.Identity.Name);
            return View(responce);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TopicsVM topic)
        {
            if (!ModelState.IsValid) return View(topic);

            var Objtopic = await _service.GetByIdAsync(id);

            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0) //file is attached
            {
                string fileExtension = Path.GetExtension(files[0].FileName);
                if (!fileExtension.Equals(".jpeg") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".png"))
                {
                    TempData["error"] = "Invalid Image format";
                    return View(topic);
                }
                //call the function
                var imageName = _service.DeleteOldAndUploadNewFile(files, Objtopic.Image.ToString());
                topic.Image = imageName;
            }
            else
            {
                topic.Image = Objtopic.Image;
            }
            await _service.UpdateTopic(id, topic);
            _logger.LogInformation("The Record is successfully modified by {0}", User.Identity.Name);
            return RedirectToAction(nameof(TopicsList), new { @id = topic.SubjectId });
        }

        //Details of a subject
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ObjTopic = await _service.GetByIdAsync(id);
            var objquestion = await _service.GetAllQuestionsById(id);
            if (ObjTopic == null) return View("NotFound");
            var responce = new TopicsVM()
            {
                Id = ObjTopic.Id,
                Code = ObjTopic.Code,
                Image = ObjTopic.Image,
                TopicText = ObjTopic.TopicText,
                Status = ObjTopic.Status,
                SubjectId = ObjTopic.SubjectId,
                Description = ObjTopic.Description,
                CreatedOn = ObjTopic.CreatedOn,
                CreatedBy = ObjTopic.CreatedBy,
                UpdatedOn = ObjTopic.UpdatedOn,
                UpdatedBy = ObjTopic.UpdatedBy,
                Questions = objquestion,
                MCQCount = ObjTopic.MCQCount,
                MCQMarks = ObjTopic.MCQMarks,
                SEQCount = ObjTopic.SEQCount,
                SEQMarks = ObjTopic.SEQMarks,
                LongQCount = ObjTopic.LongQCount,
                LongQMarks = ObjTopic.LongQMarks,
            };
            _logger.LogInformation("Details page of Topic Contorller is accessed by {0}", User.Identity.Name);
            return View(responce);
        }


        //delete a Topic
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ObjTopic = await _service.GetByIdAsync(id);
            if (ObjTopic == null) return View("NotFound");
            var responce = new TopicsVM()
            {
                Id = ObjTopic.Id,
                Code = ObjTopic.Code,
                Image = ObjTopic.Image,
                TopicText = ObjTopic.TopicText,
                Status = ObjTopic.Status,
                SubjectId = ObjTopic.SubjectId,
                Description = ObjTopic.Description,
                CreatedOn = ObjTopic.CreatedOn,
                CreatedBy = ObjTopic.CreatedBy,
                UpdatedOn = ObjTopic.UpdatedOn,
                UpdatedBy = ObjTopic.UpdatedBy
            };
            _logger.LogInformation("Delete page of Topic Contorller is accessed by {0}", User.Identity.Name);
            return View(responce);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id, int subjectId)
        {
            var ObjTopic = await _service.GetByIdAsync(id);
            if (ObjTopic == null) return View("NotFound");
            //delete the file 
            _service.DeleteFile(ObjTopic.Image.ToString());
            //delete the record
            await _service.DeleteAsync(id);
            _logger.LogInformation("The record is successfuly deleted by {0}", User.Identity.Name);
            return RedirectToAction(nameof(TopicsList), new { @id = subjectId });
        }

    }
}
