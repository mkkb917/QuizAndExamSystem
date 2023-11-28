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

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class GradeController : Controller
    {
        private readonly IGradeService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly AppDbContext _context;
        public GradeController(IGradeService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            //_context = context;
        }

        // List all grades
        public async Task<IActionResult> Index()
        {
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
            return View();
        }

        //Post: Grade/Create            create a new entry into database
        [HttpPost]
        public async Task<IActionResult> Create(Grade grade)
        {
             if (!ModelState.IsValid)
                {
                    TempData["error"] = "Invalid Model state";
                    return View(grade);
                }
            // check weather the record already exist or not
            bool checkgrade = await _service.SearchGrade(grade.GradeText.ToString());
            if(checkgrade)
            {
                TempData["error"] = "Grade Already Exists with this name";
                return View(grade);
            }
            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string Upload = webRootPath + WC.GradeImagePath;

            // pass the file name, path and file to uploader
            string fileExtension = Path.GetExtension(files[0].FileName);
            string fileName = Path.GetFileName(files[0].FileName);

            // pass the files object to funtion to save file and create thumbnail in directory
            var uploadImage = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload, fileName);
            
            grade.Image = uploadImage;
            grade.CreatedOn = DateTime.Now;
            grade.CreatedBy = User.Identity.Name;

            await _service.AddAsync(grade);
            return RedirectToAction(nameof(Index));
        }

        // view a single grade details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");
            return View(ObjGrade);
        }

        //GET: edit a grade |  Grade/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");
            return View(ObjGrade);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,Grade grade)  //[Bind("Id,Code,GradeText,Status,Description,CreatedOn,CreatedBy")]
        {
            if(id != grade.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid Model state";
                return View(grade);
            }
            grade.UpdatedBy = User.Identity.Name;
            grade.UpdatedOn = DateTime.Now.Date;
            await _service.UpdateAsync(id,grade);
            return RedirectToAction(nameof(Index));
        }

        //delete a grade
        // Get: /grade/Delete/1
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");
            return View(ObjGrade);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var ObjGrade = await _service.GetByIdAsync(id);
            if (ObjGrade == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
