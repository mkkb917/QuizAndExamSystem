﻿using ExamSystem.Data;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using ExamSystem.Models;
using System.Diagnostics;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class SubjectController : Controller
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ISubjectService _service;

        public SubjectController(ILogger<SubjectController> logger,ISubjectService service)
        {
            _logger = logger;
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
            _logger.LogInformation("Index page of Subject Controller is accessed by {0}", User.Identity.Name);
            var objSubjectVm = new SubjectsIndexVM();

            objSubjectVm.SubjectVm = (List<Subject>?)await _service.GetAllAsync();
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

            objSubjectVm.SubjectVm = await _service.GetAllActiveSubjectsById(id);
            if (objSubjectVm == null) return View("NotFound");
            foreach (var item in objSubjectVm.SubjectVm)
            {
                item.Topic = await _service.GetAllTopicsById(item.Id);
            }
            var Grade = await _service.GetGradeById(id);
            objSubjectVm.GradeId = Grade.Id;
            objSubjectVm.GradeName = Grade.GradeText;
            _logger.LogInformation("Subject List page of Subject Controller is accessed by {0}", User.Identity.Name);
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
            _logger.LogInformation("Create page of Subject Controller is accessed by {0}", User.Identity.Name);
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
            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count == 0)
            {
                TempData["error"] = "Image not attached";
                return View(subject);
            }
            // pass the file name, path and file to uploader

            string fileExtension = Path.GetExtension(files[0].FileName);
            //check the extension for image files only
            if (!fileExtension.Equals(".jpeg") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".png"))
            {
                TempData["error"] = "Invlaid Image format";
                return View(subject);
            }
            // pass the files object to funtion to save file and create thumbnail in directory
            var uploadImage = _service.DeleteOldAndUploadNewFile(files, null);
            subject.Image = uploadImage;
            await _service.AddNewSubject(subject);
            _logger.LogInformation("New record is successfully created by {0}", User.Identity.Name);
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
                Image = ObjSubject.Image,
                Description = ObjSubject.Description,
                CreatedOn = ObjSubject.CreatedOn,
                CreatedBy = ObjSubject.CreatedBy,
                UpdatedOn = ObjSubject.UpdatedOn,
                UpdatedBy = ObjSubject.UpdatedBy
            };
            var GradesListData = await _service.GetGradesList();
            ViewBag.Grades = new SelectList(GradesListData.Grades, "Id", "GradeText");
            _logger.LogInformation("Edit page of Subject Controller is accessed by {0}", User.Identity.Name);
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
            var ObjSubject = await _service.GetByIdAsync(id);

            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0) //file is attached
            {
                string fileExtension = Path.GetExtension(files[0].FileName);
                if (!fileExtension.Equals(".jpeg") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".png"))
                {
                    TempData["error"] = "Invalid Image format";
                    return View(subject);
                }
                //call the function
                var imageName = _service.DeleteOldAndUploadNewFile(files, ObjSubject.Image.ToString());
                subject.Image = imageName;
            }
            else
            {
                subject.Image = ObjSubject.Image;
            }
            await _service.UpdateSubject(id, subject);
            _logger.LogInformation("The record is successfully modified by {0}", User.Identity.Name);
            return RedirectToAction(nameof(SubjectsList), new { @id = subject.GradeId });
        }

        //Details of a subject
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ObjSubject = await _service.GetByIdAsync(id);
            var topicslist = await _service.GetAllTopicsById(id);
            if (ObjSubject == null) return View("NotFound");
            var responce = new SubjectsVM()
            {
                Id = ObjSubject.Id,
                SubjectText = ObjSubject.SubjectText,
                GradeId = ObjSubject.GradeId,
                Code = ObjSubject.Code,
                Topics = topicslist,
                Status = ObjSubject.Status,
                Image = ObjSubject.Image,
                Description = ObjSubject.Description,
                CreatedOn = ObjSubject.CreatedOn,
                CreatedBy = ObjSubject.CreatedBy,
                UpdatedOn = ObjSubject.UpdatedOn,
                UpdatedBy = ObjSubject.UpdatedBy
            };
            var TopicCounts = await _service.GetAllTopicsById(id);
            ViewBag.TopicCount = TopicCounts.Count();
            _logger.LogInformation("Details page of Subject Controller is accessed by {0}", User.Identity.Name);
            return View(responce);
        }


        //delete a Subject
        //Post: /Delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete page of Subject Controller is accessed by {0}", User.Identity.Name);
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
            //delete the file 
            _service.DeleteFile(ObjSubject.Image.ToString());
            //delete the record
            await _service.DeleteAsync(id);
            _logger.LogInformation("The Record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction(nameof(SubjectsList), new { @id = gradeId });
        }
    }
}
