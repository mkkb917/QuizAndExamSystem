using AspNetCore.ReCaptcha;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [Authorize]
    [BreadcrumbActionFilter]
    public class StudyMaterialController : Controller
    {
        private readonly IUploadsService _uploadsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public StudyMaterialController(IUploadsService uploadsService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _uploadsService = uploadsService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


      


        // GET: StudyMaterialController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

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

        // POST: StudyMaterialController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha]
        public async Task<ActionResult> Edit(int id, Uploads model)
        {

            var Obj = await _uploadsService.GetByIdAsync(id);
            if (ModelState.IsValid)
            {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    string extension = Path.GetExtension(files[0].FileName);
                    //check the extension for image files only
                    if (!(extension.Equals(".pdf")))
                    {
                        ViewBag.filetype = "Please Select a valid pdf file";
                        return View(model);
                    }
                }
                await _uploadsService.UpdateFile(id, model, files);
                return RedirectToAction("UserIndex9th", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }


        //GET: StudyMaterialController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var Obj = await _uploadsService.GetByIdAsync(id);
            if (Obj == null) return View("NotFound");
            return View(Obj);
        }


        // POST: StudyMaterialController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _uploadsService.GetByIdAsync(id);

            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _uploadsService.DeleteFile(id);

            if (Obj.Code == "9")
            {
                return RedirectToAction("UserIndex9th", new { @id = User.Identity.Name });
            }
            else if(Obj.Code == "10")
            {
                return RedirectToAction("UserIndex10th", new { @id = User.Identity.Name });
            }
            else
            {
                return View("NotFound");
            }
        }


        //[Authorize(Roles = "Admin-user")]
        // GET: StudyMaterialController/Approve
        public async Task<ActionResult> Approve(int id)
        {
            Status _status = Status.Inactive;
             //string code = "9";
            var obj = await _uploadsService.GetAllFilesByStatus(_status, id.ToString());

            return View(obj);
        }

        // POST: StudyMaterialController/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin-user")]
        public async Task<ActionResult> Approve(int id, Uploads model)
        {

            var Obj = await _uploadsService.GetByIdAsync(id);
            if (ModelState.IsValid)
            {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    string extension = Path.GetExtension(files[0].FileName);
                    //check the extension for image files only
                    if (!(extension.Equals(".pdf")))
                    {
                        ViewBag.filetype = "Please Select a valid file";
                        return View(model);
                    }
                }
                await _uploadsService.UpdateFile(id, model, files);
                return RedirectToAction("UserIndex", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }


        #region 9th class methods
        [AllowAnonymous]
        //Get: StudyMaterial/9th
        public async Task<ActionResult> Index9()
        {
            string code = "9";
            //var type = UploadsCategory.Notes;
            var Obj = await _uploadsService.GetAllFilesByStatus(Status.Active, code);
            return View(Obj);
        }


        // GET: StudyMaterial/UserIndex9th
        public async Task<IActionResult> UserIndex9th()
        {
            string code = "9";
            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, code);
            return View(obj);
        }

        public ActionResult Create9th()
        {
            var obj = new Uploads();
            return View(obj);
        }

        // POST: UploadsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha(Action = "submit")]
        public async Task<ActionResult> Create9th(Uploads model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //check the extension for image files only
                string extension = Path.GetExtension(files[0].FileName);
                if (!(extension.Equals(".pdf")))
                {
                    ViewBag.filetype = "Please Select a valid file";
                    return View(model);
                }

                // pass the model and files to Uploads service
                await _uploadsService.AddNewFile(model, files);


                return RedirectToAction("UserIndex9th", new { @id = User.Identity.Name });
            }
            return View("NotFound");
        }



        #endregion


        #region 10th class methods

        [AllowAnonymous]
        //Get: StudyMaterial/10th
        public async Task<ActionResult> Index10()
        {
            string code = "10";
            //var type = UploadsCategory.Notes;
            var Obj = await _uploadsService.GetAllFilesByStatus(Status.Active, code);
            return View(Obj);
        }

        // GET: StudyMaterial/UserIndex9th
        public async Task<IActionResult> UserIndex10th()
        {
            var code = "10";
            var user = _userManager.GetUserName(User);
            var obj = await _uploadsService.GetAllFilesByUser(user, code);
            return View(obj);
        }

        public ActionResult Create10th()
        {
            var obj = new Uploads();
            return View(obj);
        }

        // POST: UploadsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha(Action = "submit")]
        public async Task<ActionResult> Create10th(Uploads model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //check the extension for image files only
                string extension = Path.GetExtension(files[0].FileName);
                if (!(extension.Equals(".pdf")))
                {
                    ViewBag.filetype = "Please Select a valid file";
                    return View(model);
                }

                // pass the model and files to Uploads service
                await _uploadsService.AddNewFile(model, files);


                return RedirectToAction("UserIndex9th", new { @id = User.Identity.Name });
            }
            return View("NotFound");
        }



        #endregion


    }
}
