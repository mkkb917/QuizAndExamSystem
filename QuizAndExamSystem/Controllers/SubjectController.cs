using ExamSystem.Data;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service) 
        {
            _service = service;
        }

        public async void FetchGrade(int id)
        {
            var Grade = await _service.GetGradeById(id);
            TempData["GradeName"] = Grade.GradeText;
            TempData["GradeId"] = Grade.Id;
        }


        public async Task<IActionResult> Index()
        {

            var objSubjectVm = new SubjectsIndexVM();

            objSubjectVm.SubjectVm = (List<Subject>?) await _service.GetAllAsync();
            if (objSubjectVm == null) return View("NotFound");
            foreach (var item in objSubjectVm.SubjectVm)
            {
                item.Topic = await _service.GetAllTopicsById(item.Id);
                item.Grade = await _service.GetGradeById(item.GradeId);
            }

            return View(objSubjectVm);
        }

        // Display by Id

        public async Task<IActionResult> SubjectsList(int id)
        {
            
            var objSubjectVm = new SubjectsIndexVM();

            objSubjectVm.SubjectVm = await _service.GetAllSubjectsById(id);
            if (objSubjectVm == null) return View("NotFound");
            foreach (var item in objSubjectVm.SubjectVm)
            {
                item.Topic = await _service.GetAllTopicsById(item.Id);
            }
            var Grade = await _service.GetGradeById(id);
            objSubjectVm.GradeId = Grade.Id;
            objSubjectVm.GradeName = Grade.GradeText;

            return View(objSubjectVm);
        }

        //Get: Create New Subject
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if (id != null)
            {
                TempData["GradeId"] = id;
            }
            else
            {
                var GradesListData = await _service.GetGradesList();
                ViewBag.Grades = new SelectList(GradesListData.Grades, "Id", "GradeText");
            }
            return View();
        }

        //Post: Subject/Create            create a new entry into database
        [HttpPost]
        public async Task<IActionResult> Create(SubjectsVM subject)
        {
            if (!ModelState.IsValid)
            {
                var GradesListData = await _service.GetGradesList();
                ViewBag.Grades = new SelectList(GradesListData.Grades, "Id", "GradeText");

                return View(subject);
            }
            await _service.AddNewSubject(subject);
            return RedirectToAction("SubjectsList", new { @id = subject.GradeId });
        }

        //Get: .../edit/id         Update a Subject
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var ObjSubject = await _service.GetByIdAsync(id);
            if (ObjSubject == null) return View("NotFound");
            var responce = new SubjectsVM()
            {
                Id = ObjSubject.Id,
                SubjectText = ObjSubject.SubjectText,
                GradeId = ObjSubject.GradeId,
                Description = ObjSubject.Description,
                CreatedOn = ObjSubject.CreatedOn,
                CreatedBy = ObjSubject.CreatedBy,
                UpdatedOn = ObjSubject.UpdatedOn,
                UpdatedBy = ObjSubject.UpdatedBy
            };
            var GradesListData = await _service.GetGradesList();
            ViewBag.Grades = new SelectList(GradesListData.Grades, "Id", "GradeText");
            return View(responce);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SubjectsVM subject)
        {
            if (!ModelState.IsValid)
            {
                var GradesListData = await _service.GetGradesList();
                ViewBag.Grades = new SelectList(GradesListData.Grades, "Id", "GradeText");
                return View(subject);
            }
            await _service.UpdateSubject(id, subject);
            return RedirectToAction(nameof(SubjectsList), new { @id = subject.GradeId });
        }

        //Details of a subject
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ObjSubject = await _service.GetByIdAsync(id);
            if (ObjSubject == null) return View("NotFound");
            var responce = new SubjectsVM()
            {
                Id = ObjSubject.Id,
                SubjectText = ObjSubject.SubjectText,
                GradeId = ObjSubject.GradeId,
                Code= ObjSubject.Code,
                Status= ObjSubject.Status,
                Description = ObjSubject.Description,
                CreatedOn = ObjSubject.CreatedOn,
                CreatedBy = ObjSubject.CreatedBy,
                UpdatedOn = ObjSubject.UpdatedOn,
                UpdatedBy = ObjSubject.UpdatedBy
            };
            var TopicCounts = await _service.GetAllTopicsById(id);
            ViewBag.TopicCount = TopicCounts.Count();
            return View(responce);
        }


        //delete a Subject
        //Post: /Delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var TopicCounts = await _service.GetAllTopicsById(id);
            ViewBag.TopicCount = TopicCounts.Count();

            var ObjSubject = await _service.GetByIdAsync(id);
            if (ObjSubject == null) return View("NotFound");
            return View(ObjSubject);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id, int gradeId)
        {
            var ObjSubject = await _service.GetByIdAsync(id);
            if (ObjSubject == null) return View("NotFound");
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(SubjectsList), new { @id = gradeId });
        }
    }
}
