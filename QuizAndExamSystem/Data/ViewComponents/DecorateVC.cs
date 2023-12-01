using ExamSystem.Data.Services;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace ExamSystem.Data.ViewComponents
{
    public class DecorateViewComponent : ViewComponent
    {
        private readonly IUploadsService _uploadsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public DecorateViewComponent(IUploadsService uploadsService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _uploadsService = uploadsService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //pass the staus and upload category as Code 
            var obj = await _uploadsService.GetFilesByCategoryForComponenet(Status.Active, UploadsCategory.Decorate);
            return View(obj);
            ;
        }
    }
}
