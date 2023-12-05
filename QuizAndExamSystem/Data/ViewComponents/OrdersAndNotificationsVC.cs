using ExamSystem.Data.Interface;
using ExamSystem.Data.Services;
using ExamSystem.Data.Static;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Data.ViewComponents
{
    public class OrdersAndNotificationsViewComponent:ViewComponent
    {
        private readonly ISEDService _service;
        public OrdersAndNotificationsViewComponent(ISEDService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //pass the staus and upload category as Code 
            var obj = await _service.GetAllFilesByStatus(Status.Active);
            return View(obj);

        }
    }
}
