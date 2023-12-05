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
        private readonly ILogger<SEDController> _logger;
        private readonly ISEDService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public SEDController(ILogger<SEDController> logger ,ISEDService service, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        // GET: SEDsController
        public async Task<ActionResult> Index()
        {
            var Obj = await _service.GetAllFiles();
            _logger.LogInformation("Index page of SED Controller is accessed by {0}", User.Identity.Name);
            return View(Obj);
        }

        // GET: SEDsController/UserSED
        public async Task<IActionResult> UserSED()
        {
            var user = _userManager.GetUserName(User);
            var obj = await _service.GetAllFilesByUser(user);
            _logger.LogInformation("User Index page of SED Controller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // GET: SEDsController/Create
        public ActionResult Create()
        {
            var obj = new SED();
            _logger.LogInformation("Create page of SED Controller is accessed by {0}", User.Identity.Name);
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
                _logger.LogInformation("Create page of SED Controller is accessed by {0}", User.Identity.Name);
                await _service.AddNewFile(sed, files);
                _logger.LogInformation("New record is successfully created by {0}", User.Identity.Name);

                return RedirectToAction("UserSED", new { @id = User.Identity.Name });
            }
            return View("NotFound");
        }

        // GET: SEDsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            _logger.LogInformation("Edit page of SED Controller is accessed by {0}", User.Identity.Name);
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
                _logger.LogInformation("Edit page of SED Controller is accessed by {0}", User.Identity.Name);
                _logger.LogInformation("Th record is successfully modified by {0}", User.Identity.Name);
                return RedirectToAction("UserSED", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }



        // POST: SEDsController/Delete/5

        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _service.GetByIdAsync(id);
            _logger.LogInformation("Delete page of SED Controller is accessed by {0}", User.Identity.Name);
            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _service.DeleteFile(id);
            _logger.LogInformation("The Record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction("UserSED", new { @id = User.Identity.Name });
        }


        // GET: SEDsController/Approve
        public async Task<ActionResult> Approve()
        {
            Status _status = Status.Pending;
            var obj = await _service.GetAllFilesByStatus(_status);
            _logger.LogInformation("Approve page of SED Controller is accessed by {0}", User.Identity.Name);
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
                _logger.LogInformation("record is successfully approved by {0}", User.Identity.Name);
                return RedirectToAction("UserSED", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }
    }
}
