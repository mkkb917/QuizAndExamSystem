using ExamSystem.Data.Interface;
using ExamSystem.Data.Static;
using ExamSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Data.ViewComponents
{
    public class BooksViewComponent : ViewComponent
    {
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public BooksViewComponent(IBookService bookService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _bookService = bookService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //pass the staus and upload category as Code 
            var obj = await _bookService.GetBooksByCategoryForComponenet(Status.Active);
            return View(obj);
            
        }
    }
}
