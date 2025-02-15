﻿using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [Authorize]
    [BreadcrumbActionFilter]
    public class CornerController : Controller
    {
        private readonly ILogger<CornerController> _logger;
        private readonly IUploadsService _uploadsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public CornerController(ILogger<CornerController> logger,IUploadsService uploadsService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _uploadsService = uploadsService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


        public string ReturnRedirectActionName(UploadsCategory? fileType)
        {
            string actionName;
            switch (fileType)
            {
                case UploadsCategory.Calender:
                    actionName = "UserCalender";
                    break;
                case UploadsCategory.Decorate:
                    actionName = "UserDecorate";
                    break;
                case UploadsCategory.Events:
                    actionName = "UserEvents";
                    break;
                case UploadsCategory.Manage:
                    actionName = "UserManage";
                    break;
                case UploadsCategory.Notes:
                    actionName = "UserNotes";
                    break;
                case UploadsCategory.PastPapers:
                    actionName = "UserPastPapers";
                    break;
                case UploadsCategory.Syllabus:
                    actionName = "UserSyllabus";
                    break;
                default:
                    actionName = "Index";
                    break;
            }
            return actionName;
        }


        public ActionResult Create()
        {
            _logger.LogInformation("Create page of Corner Controller is requested by {0}", User.Identity.Name);
            var obj = new Uploads();
            return View(obj);
        }

        // POST: UploadsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha(Action = "submit")]
        public async Task<ActionResult> Create(Uploads model)
        {
            _logger.LogInformation("Create page of Corner Controller is accessed by {0}", User.Identity.Name);
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //check the extension for image files only
                string extension = Path.GetExtension(files[0].FileName);
                if ((extension.Equals(".jpeg")) || (extension.Equals(".jpg")) || (extension.Equals(".pdf")))
                {
                    // pass the model and files to Uploads service
                    await _uploadsService.AddNewFile(model, files);

                    // return to page of category
                    string actionName;
                    actionName = ReturnRedirectActionName(model.FileType);
                    //return RedirectToAction(actionName, "Corner", new { @id = User.Identity.Name });
                    _logger.LogInformation("New record is successfully created by {0}", User.Identity.Name);
                    return RedirectToAction(actionName, "Corner");

                }
                ViewBag.filetype = "Please Select a valid file";
                return View(model);
            }
            return View("NotFound");
        }



        // GET: CornerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit page of Corner Controller is requested by {0}", User.Identity.Name);
            var Obj = await _uploadsService.GetByIdAsync(id);

            var responce = new Uploads()
            {
                Id = Obj.Id,
                Title = Obj.Title,
                Code = Obj.Code,
                Status = Obj.Status,
                Author = Obj.Author,
                FileName = Obj.FileName,
                FileThumbnail = Obj.FileThumbnail,
                FileType = Obj.FileType,
                Description = Obj.Description,
                CreatedOn = Obj.CreatedOn,
                CreatedBy = Obj.CreatedBy,
                UpdatedOn = Obj.UpdatedOn,
                UpdatedBy = Obj.UpdatedBy
            };
            return View(responce);
        }

        // POST: CornerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha]
        public async Task<ActionResult> Edit(int id, Uploads model)
        {
            _logger.LogInformation("Edit page of Corner Controller is accessed by {0}", User.Identity.Name);
            var Obj = await _uploadsService.GetByIdAsync(id);
            if (ModelState.IsValid)
            {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    string extension = Path.GetExtension(files[0].FileName);
                    //check the extension for image files only
                    if ((extension.Equals(".jpeg")) || (extension.Equals(".jpg")) || (extension.Equals(".pdf")))
                    {
                        await _uploadsService.UpdateFile(id, model, files);
                        // return to page of category
                        string actionName;
                        actionName = ReturnRedirectActionName(model.FileType);
                        _logger.LogInformation("Record is successfully updated by {0}", User.Identity.Name);
                        return RedirectToAction(actionName, "Corner");
                    }
                }

                ViewBag.filetype = "Please Select a valid file";
                return View(model);
            }
            return View(nameof(NotFound));
        }


        //GET: CornerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            _logger.LogInformation("Detail page of Corner Controller is requested by {0}", User.Identity.Name);
            var Obj = await _uploadsService.GetByIdAsync(id);
            if (Obj == null) return View("NotFound");
            return View(Obj);
        }


        // POST: CornerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _uploadsService.GetByIdAsync(id);
            _logger.LogInformation("Delete page of Corner Controller is requested by {0}", User.Identity.Name);
            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _uploadsService.DeleteFile(id);

            // return to page of category
            string actionName;
            actionName = ReturnRedirectActionName(Obj.FileType);
            _logger.LogInformation("record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction(actionName, "Corner");

        }

        public IActionResult GetEnums(int id)
        {
            try
            {
                string enumNamespace = "ExamSystem.Data.Static";
                // conver the enum value to its type name (typecast)
                var category = ((UploadsCategory)id).ToString();
                if (!string.IsNullOrEmpty(category))
                {
                    string enumTypeName = $"{enumNamespace}.{category}Enum";
                    // Assume you have CategoryEnum, CarEnum, AnimalEnum, HumanEnum defined
                    Type enumType = Type.GetType(enumTypeName);

                    if (enumType != null && enumType.IsEnum)
                    {
                        var enumValues = Enum.GetNames(enumType);

                        return Json(enumValues);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                return BadRequest("Error: " + ex.Message);
            }

            return BadRequest("Invalid category");
        }


        //[Authorize(Roles = "Admin-user")]
        // GET: CornerController/Approve
        [HttpGet]
        public async Task<ActionResult> Approve()
        {
            _logger.LogInformation("Approve page of Corner Controller is requested by {0}", User.Identity.Name);
            Status _status = Status.Pending;
            var obj = await _uploadsService.GetAllFilesByStatus(_status);
            ViewBag.CurrentAction = "Approve";
            return View(obj);
        }

        // POST: CornerController/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin-user")]
        public async Task<ActionResult> Approve(int id)
        {
            if (id == 0)
            {
                return View(nameof(NotFound));
            }
            await _uploadsService.ApproveFile(id);
            _logger.LogInformation("Approve page of Corner Controller is accessed by {0}", User.Identity.Name);
            _logger.LogInformation("Data is successfully approved by {0}", User.Identity.Name);
            return RedirectToAction("Approve");
        }



        #region School Decoration
        [AllowAnonymous]
        public async Task<IActionResult> Decorate()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.Decorate);
            _logger.LogInformation("Decorate page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // GET: CornerController/UserDecoration
        public async Task<IActionResult> UserDecorate()
        {

            var user = _userManager.GetUserName(User);
            //int category = (int)UploadsCategory.Decoration;
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.Decorate);
            _logger.LogInformation("User Decorate page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion

        #region School Syllabus Books
        [AllowAnonymous]
        public async Task<IActionResult> Syllabus()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.Syllabus);
            _logger.LogInformation("Syllabus page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // GET: CornerController/UserSyllabus
        public async Task<IActionResult> UserSyllabus()
        {

            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.Syllabus);
            _logger.LogInformation("user Syllabus page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion

        #region School Events 
        [AllowAnonymous]
        public async Task<IActionResult> Events()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.Events);
            _logger.LogInformation("School Events page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }


        // GET: CornerController/UserSyllabus
        public async Task<IActionResult> UserEvents()
        {

            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.Events);
            _logger.LogInformation("User School Events page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion

        #region School management data for administration 
        [AllowAnonymous]
        public async Task<IActionResult> Manage()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.Events);
            _logger.LogInformation("School manage page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }


        // GET: CornerController/UserManage
        public async Task<IActionResult> UserManage()
        {

            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.Events);
            _logger.LogInformation("User school manage page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion

        #region  Educational Calanders 
        [AllowAnonymous]
        public async Task<IActionResult> Calender()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.Calender);
            _logger.LogInformation("Educational calender page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // GET: CornerController/UserManage
        public async Task<IActionResult> UserCalender()
        {

            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.Calender);
            _logger.LogInformation("User Educational Calandar page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion

        #region  Notes for Students 
        [AllowAnonymous]
        public async Task<IActionResult> Notes()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.Notes);
            _logger.LogInformation("Student Notes page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // GET: CornerController/UserNotes
        public async Task<IActionResult> UserNotes()
        {

            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.Notes);
            _logger.LogInformation("user student Notes page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion

        #region  PastPapers for Students 
        [AllowAnonymous]
        public async Task<IActionResult> PastPapers()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetAllFilesByCategory(Status.Active, UploadsCategory.PastPapers);
            _logger.LogInformation("student PastPaper page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // GET: CornerController/UserNotes
        public async Task<IActionResult> UserPastPapers()
        {
            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, UploadsCategory.PastPapers);
            _logger.LogInformation("User Student PastPaper page of Corner Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        #endregion
    }
}
