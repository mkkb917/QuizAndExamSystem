using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Data.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using ExamSystem.Filters;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    [Authorize]
    [BreadcrumbActionFilter]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationsController(INotificationService service, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


        // list all notifications
        public async Task<IActionResult> Index()
        {
            var ObjAllNotifications = await _service.GetAllNotification();
            return View(ObjAllNotifications);
        }

        // all notification by user
        public async Task<IActionResult> UserNotification()
        {
            var user = _userManager.GetUserName(User);
            var obj = await _service.GetAllNotificationByUser(user);
            return View(obj);
        }

        //Get: Create New Notification
        [HttpGet]
        public IActionResult Create()
        {
            var newNotification = new Notification();
            return View(newNotification);
            //return View();
        }

        //Post: Notifications/Create      
        [HttpPost]
        public async Task<IActionResult> Create(Notification notification)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                //check the extension for image files only
                string extension = Path.GetExtension(files[0].FileName);
                if(! (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png") || extension.Equals(".gif")))
                {
                    ViewBag.filetype = "Please select an image file";
                    return View(notification);
                }


                notification.CreatedBy = User.Identity.Name;
                notification.CreatedOn = DateTime.Today;

                // pass the model and files to notification service
                await _service.AddNewNotification(notification, files);

                return RedirectToAction("UserNotification", new { @id = User.Identity.Name });
            }
            return View("NotFound");

        }

        //Get: .../edit/id       
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //IEnumerable<SelectListItem> statusDropDown ;
            var ObjNotification = await _service.GetByIdAsync(id);
            //if (ObjNotification == null) return View("NotFound");
            //string upload = _webHostEnvironment.WebRootPath + WC.NotificationPath;
            
            var responce = new Notification()
            {
                Id = ObjNotification.Id,
                Code = ObjNotification.Code,
                IssuedBy = ObjNotification.IssuedBy,
                NotificationDate = ObjNotification.NotificationDate,
                NotificationFile = ObjNotification.NotificationFile,
                Description = ObjNotification.Description,
                CreatedOn = ObjNotification.CreatedOn,
                CreatedBy = ObjNotification.CreatedBy,
                UpdatedOn = ObjNotification.UpdatedOn,
                UpdatedBy = ObjNotification.UpdatedBy
            };
            return View(responce);
        }
        //[Bind("Id", "NotificationFile", "NotificationDate", "IssuedBy", "Status", "Code", "Description", "")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id,IFormFile upload, Notification notification)
        {
            var Obj = await _service.GetByIdAsync(id);
            if (ModelState.IsValid)
            {
                //grab the file
                var files = Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                string Upload = webRootPath + WC.NotificationPath;
                var oldfile = Path.Combine(webRootPath, notification.NotificationFile);
                if (files.Count > 0)
                {
                    //check if the file already exists then delete the old file
                    if (System.IO.File.Exists(oldfile))
                    {
                        System.IO.File.Delete(oldfile);
                    }
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    // copy the file in system folder
                    using (var fileStream = new FileStream(Path.Combine(Upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    await _service.UpdateNotification(id, notification, fileName + extension);
                    return RedirectToAction("UserNotification", new { @id = User.Identity.Name });
                }
                else
                {
                    await _service.UpdateNotification(id, notification, notification.NotificationFile);
                    return RedirectToAction("UserNotification", new { @id = User.Identity.Name });
                }
            }
            return View(nameof(NotFound));
        }

        //delete a notification
        //Post: /Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            var Obj = await _service.GetByIdAsync(id);

            // delete the saved file from directory
            string upload = _webHostEnvironment.WebRootPath + WC.NotificationPath;
            var file = Path.Combine(upload, Obj.NotificationFile);
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }
            // delete the record from database
            if (Obj == null) return View("NotFound");
            await _service.DeleteAsync(id);
            return RedirectToAction("UserNotification", new { @id = User.Identity.Name });

        }
    }
}
