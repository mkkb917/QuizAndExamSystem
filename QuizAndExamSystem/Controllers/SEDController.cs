using AspNetCore.ReCaptcha;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class SEDController : Controller
    {
        private readonly ISEDService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public SEDController(ISEDService service, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        // GET: SEDsController
        public async Task<ActionResult> Index()
        {
            var Obj = await _service.GetAllFiles();
            return View(Obj);
        }

        // GET: SEDsController/UserSED
        public async Task<IActionResult> UserSED()
        {
            var user = _userManager.GetUserName(User);
            var obj = await _service.GetAllFilesByUser(user);
            return View(obj);
        }

        // GET: SEDsController/Create
        public ActionResult Create()
        {
            var obj = new SED();
            return View(obj);
        }

        // POST: SEDsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha(Action = "submit")]
        public async Task<ActionResult> Create(SED sed)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //check the extension for image files only
                string extension = Path.GetExtension(files[0].FileName);
                if (!(extension.Equals(".pdf")))
                {
                    ViewBag.filetype = "Please Select a valid file";
                    return View(sed);
                }

                //create the faceImage thumbnail for that file
                sed.FileFaceImage = WC.PDF_SED_Path + "index.jpg";
                // await _service.AddNewFaceFile(files);

                // pass the model and files to notification service
                await _service.AddNewFile(sed, files);


                return RedirectToAction("UserSED", new { @id = User.Identity.Name });
            }
            return View("NotFound");
        }

        // GET: SEDsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            var Obj = await _service.GetByIdAsync(id);

            var responce = new SED()
            {
                Id = Obj.Id,
                Title = Obj.Title,
                Code = Obj.Code,
                IssuedBy = Obj.IssuedBy,
                Date = Obj.Date,
                FileName = Obj.FileName,
                FileFaceImage = Obj.FileFaceImage,
                Description = Obj.Description,
                CreatedOn = Obj.CreatedOn,
                CreatedBy = Obj.CreatedBy,
                UpdatedOn = Obj.UpdatedOn,
                UpdatedBy = Obj.UpdatedBy
            };
            return View(responce);
        }

        // POST: SEDsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public async Task<ActionResult> Edit(int id, SED sed)
        {

            var Obj = await _service.GetByIdAsync(id);
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
                        return View(sed);
                    }
                }
                await _service.UpdateFile(id, sed, files);
                return RedirectToAction("UserSED", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }



        // POST: SEDsController/Delete/5

        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _service.GetByIdAsync(id);

            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _service.DeleteFile(id);
            return RedirectToAction("UserSED", new { @id = User.Identity.Name });
        }


        // GET: SEDsController/Approve
        public async Task<ActionResult> Approve()
        {
            Status _status = Status.Inactive;
            var obj = await _service.GetAllFilesByStatus(_status);
            
            return View(obj);
        }

        // POST: SEDsController/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(int id, SED sed)
        {

            var Obj = await _service.GetByIdAsync(id);
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
                        return View(sed);
                    }
                }
                await _service.UpdateFile(id, sed, files);
                return RedirectToAction("UserSED", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }
    }
}
