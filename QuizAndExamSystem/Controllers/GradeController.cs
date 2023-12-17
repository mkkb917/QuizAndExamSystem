using ExamSystem.Data;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Models;
using Microsoft.AspNetCore.Hosting;
using ExamSystem.Extensions;
using NuGet.Packaging.Signing;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class GradeController : Controller
    {
        private readonly ILogger<GradeController> _logger;
        private readonly IGradeService _service;
        private readonly string webRootPath;
        public GradeController(ILogger<GradeController> logger,IGradeService service, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _service = service;
            webRootPath = webHostEnvironment.WebRootPath;
            
        }

        // List all grades
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index page of Grade Controller is accessed by {0}", User.Identity.Name);
            var obj = new List<Grade>();
            obj = (List<Grade>)await _service.GetAllAsync();
            foreach (var item in obj)
            {
                item.Subject = await _service.GetSubjectById(item.Id);
            }
            return View(obj);
        }

        // Get: Grade/Create
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Create page of Grade Controller is requested by {0}", User.Identity.Name);
            return View();
        }

        //Post: Grade/Create            create a new entry into database
        [HttpPost]
        public async Task<IActionResult> Create(Grade grade)
        {
            _logger.LogInformation("Create page of Grade Controller is accessed by {0}", User.Identity.Name);
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid Model state";
                return View(grade);
            }
            // check weather the record already exist or not
            bool checkgrade = await _service.SearchGrade(grade.GradeText.ToString());
            if (checkgrade)
            {
                TempData["error"] = "Grade Already Exists with this name";
                return View(grade);
            }

            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count == 0)
            {
                TempData["error"] = "Image not attached";
                return View(grade);
            }
            // pass the file name, path and file to uploader

            string fileExtension = Path.GetExtension(files[0].FileName);

            //check the extension for image files only
            if (!fileExtension.Equals(".jpeg") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".png"))
            {
                TempData["error"] = "Invlaid Image format";
                return View(grade);
            }
            // pass the files object to funtion to save file and create thumbnail in directory
            var uploadImage = _service.DeleteOldAndUploadNewFile(files, null);
            grade.Image = uploadImage;
            grade.CreatedOn = DateTime.Now;
            grade.CreatedBy = User.Identity.Name;

            await _service.AddAsync(grade);
            _logger.LogInformation("New record is added/created by {0}", User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        // view a single grade details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Detail page of Grade Controller is accessed by {0}", User.Identity.Name);
            var ObjGrade = await _service.GetByIdAsync(id);
            ObjGrade.Subject = await _service.GetSubjectById(ObjGrade.Id);
            if (ObjGrade == null) return View("NotFound");
            return View(ObjGrade);
        }

        //GET: edit a grade |  Grade/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit page of Grade Controller is requested by {0}", User.Identity.Name);
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");
            return View(ObjGrade);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Grade grade)  //[Bind("Id,Code,GradeText,Status,Description,CreatedOn,CreatedBy")]
        {
            _logger.LogInformation("Edit page of Grade Controller is accessed by {0}", User.Identity.Name);
            if (id != grade.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid Model state";
                return View(grade);
            }
            var ObjGrade = await _service.GetByIdAsync(id);

            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0) //file is attached
            {
                string fileExtension = Path.GetExtension(files[0].FileName);
                if (!fileExtension.Equals(".jpeg") && !fileExtension.Equals(".jpg") && !fileExtension.Equals(".png"))
                {
                    TempData["error"] = "Invalid Image format";
                    return View(grade);
                }
                //call the function
                var imageName = _service.DeleteOldAndUploadNewFile(files, ObjGrade.Image.ToString());
                grade.Image = imageName;
            }
            else
            {
                grade.Image = ObjGrade.Image;
            }
            grade.UpdatedBy = User.Identity.Name;
            grade.UpdatedOn = DateTime.Now.Date;
            await _service.UpdateAsync(id, grade);
            _logger.LogInformation("The Record is updated successfully by {0}", User.Identity.Name);
            return RedirectToAction(nameof(Details),new { @id = grade.Id });
        }

        //delete a grade
        // Get: /grade/Delete/1
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete page of Grade Controller is accessed by {0}", User.Identity.Name);
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");
            return View(ObjGrade);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");
            //delete the file 
             _service.DeleteFile(ObjGrade.Image.ToString());
            //delete the record
            await _service.DeleteAsync(id);
            _logger.LogInformation("the record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

    }
}
