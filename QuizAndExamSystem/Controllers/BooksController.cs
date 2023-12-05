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
    [BreadcrumbActionFilter]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public BooksController(ILogger<BooksController> logger, IBookService service, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: BooksController
        public async Task<ActionResult> Index()
        {
            var Obj = await _service.GetAllBooks();
            _logger.LogInformation("Indexpage of Books controller is accessed by {1}", User.Identity.Name);
            return View(Obj);
        }

        // GET: BooksController/UserIndex
        public async Task<IActionResult> UserIndex()
        {
            var user = _userManager.GetUserName(User);
            var obj = await _service.GetAllBooksByUser(user);
            _logger.LogInformation("Userindex of Books controller is accessed by {1}", User.Identity.Name);
            return View(obj);
        }

        // GET: BooksController/Create
        public ActionResult Create()
        {
            var obj = new Books();
            _logger.LogInformation("create method is requested to accessed by {1}", User.Identity.Name);
            return View(obj);
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateReCaptcha(Action = "submit")]
        public async Task<ActionResult> Create(Books book)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Create function of Books controller is accessed by {1}", User.Identity.Name);
                var files = HttpContext.Request.Form.Files;
                //check the extension for image files only
                string extension = Path.GetExtension(files[0].FileName);
                if (!(extension.Equals(".pdf")))
                {
                    ViewBag.filetype = "Please Select a valid file";
                    return View(book);
                }

                //create the faceImage thumbnail for that file
                book.FileThumbnail = WC.PDF_Book_Path + "index.jpg";
                // await _service.AddNewFaceFile(files);

                // pass the model and files to notification service
                await _service.AddNewBook(book, files);
                _logger.LogInformation("new record is added by calling Create function of Books controller is accessed by {1}", User.Identity.Name);

                return RedirectToAction("UserIndex", new { @id = User.Identity.Name });
            }
            return View("NotFound");
        }

        // GET: BooksController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            var Obj = await _service.GetByIdAsync(id);
            _logger.LogInformation("Edit page of Books controller is requested by {1}", User.Identity.Name);
            var responce = new Books()
            {
                Id = Obj.Id,
                Title = Obj.Title,
                Code = Obj.Code,
                FileName = Obj.FileName,
                FileThumbnail = Obj.FileThumbnail,
                Description = Obj.Description,
                CreatedOn = Obj.CreatedOn,
                CreatedBy = Obj.CreatedBy,
                UpdatedOn = Obj.UpdatedOn,
                UpdatedBy = Obj.UpdatedBy
            };
            return View(responce);
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public async Task<ActionResult> Edit(int id, Books book)
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
                        return View(book);
                    }
                }
                await _service.UpdateBook(id, book, files);
                _logger.LogInformation("Edit page of Books controller is accessed by {1}", User.Identity.Name);
                _logger.LogInformation("Record is updated successfully by ", User.Identity.Name);
                return RedirectToAction("UserIndex", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }

        //GET: BooksController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var Obj = await _service.GetByIdAsync(id);
            _logger.LogInformation("Detail page of Books controller is requested by {1}", User.Identity.Name);
            if (Obj == null) return View("NotFound");
            return View(Obj);
        }


        // POST: BooksController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var Obj = await _service.GetByIdAsync(id);
            _logger.LogInformation("Delete page of Books controller is accessed by {1}", User.Identity.Name);
            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _service.DeleteBook(id);
            _logger.LogInformation("record is successfully deleted by {0}", User.Identity.Name);
            return RedirectToAction("UserIndex", new { @id = User.Identity.Name });
        }


        [Authorize(Roles = "Admin-user")]
        // GET: BooksController/Approve
        public async Task<ActionResult> Approve()
        {
            Status _status = Status.Pending;
            var obj = await _service.GetAllBooksByStatus(_status);
            _logger.LogInformation("Approve page of Books Controller is requested by ",User.Identity.Name);
            return View(obj);
        }

        // POST: BooksController/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin-user")]
        public async Task<ActionResult> Approve(int id, Books book)
        {
            _logger.LogInformation("Approve page of Books controller is accessed by {1}", User.Identity.Name);
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
                        return View(book);
                    }
                }
                await _service.UpdateBook(id, book, files);
                _logger.LogInformation("Record is successfully approved by {1}", User.Identity.Name);
                return RedirectToAction("UserIndex", new { @id = User.Identity.Name });
            }
            return View(nameof(NotFound));
        }
    }
}
