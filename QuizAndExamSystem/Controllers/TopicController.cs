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
        //private readonly AppDbContext _context;

        public TopicController(ITopicService service) //, AppDbContext context)
        {
            _service = service;
            //_context = context;
        }
        public async Task<IActionResult> Index(int Id)
        {
            var obj = new List<TopicsVM>();
            obj = (List<TopicsVM>)await _service.GetAllAsync();
            foreach (var item in obj)
            {
                item.Questions = await _service.GetAllQuestionsById(item.Id);
            }
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
                item.SubjectId= subject.Id;
            }
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
           
            await _service.AddNewTopic(topic);
            return RedirectToAction(nameof(TopicsList),new {@id=topic.SubjectId});
            
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
                Code=Obj.Code,
                TopicText = Obj.TopicText,
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
            return View(responce);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TopicsVM topic)
        {
            if (!ModelState.IsValid) return View(topic);
            
            await _service.UpdateTopic(id, topic);
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
                TopicText = ObjTopic.TopicText,
                Status = ObjTopic.Status,
                SubjectId = ObjTopic.SubjectId,
                Description = ObjTopic.Description,
                CreatedOn = ObjTopic.CreatedOn,
                CreatedBy = ObjTopic.CreatedBy,
                UpdatedOn = ObjTopic.UpdatedOn,
                UpdatedBy = ObjTopic.UpdatedBy,
                Questions = objquestion,
            };
            //var QuestionCounts = await _service.GetAllTopicsById(id);
            //ViewBag.QuestionCount = QuestionCounts.Count();
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
                TopicText = ObjTopic.TopicText,
                Status = ObjTopic.Status,
                SubjectId = ObjTopic.SubjectId,
                Description = ObjTopic.Description,
                CreatedOn = ObjTopic.CreatedOn,
                CreatedBy = ObjTopic.CreatedBy,
                UpdatedOn = ObjTopic.UpdatedOn,
                UpdatedBy = ObjTopic.UpdatedBy
            };
            return View(responce);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id, int subjectId)
        {
            var ObjTopic = await _service.GetByIdAsync(id);
            if (ObjTopic == null) return View("NotFound");
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(TopicsList), new { @id = subjectId});
        }

    }
}
