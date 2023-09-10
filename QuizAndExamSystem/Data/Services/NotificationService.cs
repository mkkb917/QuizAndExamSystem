using ExamSystem.Data.Base;
using Microsoft.EntityFrameworkCore;
using ExamSystem.Data.Interface;
using ExamSystem;
using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Services
{
    public class NotificationService : EntityBaseRepository<Notification>, INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public NotificationService(AppDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            
        }

        public async Task AddNewNotification(Notification data, IFormFileCollection files)
        {
            // upload the file
            if (files != null && data != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string profileUpload = webRootPath + WC.NotificationPath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                
                // copy the file in system folder
                using (var fileStream = new FileStream(Path.Combine(profileUpload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                var newNotification = new Notification()
                {
                    Code = data.Code,
                    IssuedBy = data.IssuedBy,
                    Status = data.Status,
                    NotificationFile = fileName + extension,
                    NotificationDate = data.NotificationDate,
                    Description = data.Description,
                    CreatedOn = DateTime.Now.Date,
                    CreatedBy = data.CreatedBy,
                };

                //sve the data to the database
                await _context.Notification.AddAsync(newNotification);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteNotification(int id)
        {
            // delete the choice table entry
            var ObjNotification = await _context.Notification.FirstOrDefaultAsync(n => n.Id == id);
            if (ObjNotification != null)
            {
                // delete the record
                _context.Remove(ObjNotification);
                await _context.SaveChangesAsync();

                // delete the uploaded file from directory

            }
        }

        public async Task<List<Notification>> GetAllNotification()
        {
            var responce = await _context.Notification.ToListAsync();
            return responce;
        }

        public async Task<List<Notification>> GetAllNotificationByUser(string Id)
        {
            var responce = await _context.Notification.Where(n => n.CreatedBy == Id).ToListAsync();
            return responce;
        }

        public Task<DropDownsListsVM> GetDDLObject()
        {
            throw new NotImplementedException();
        }

        public async Task<Notification> GetNotificationById(int id)
        {
            var responce = await _context.Notification.Where(n => n.Id == id).FirstOrDefaultAsync();
            return responce;
        }

        public async Task UpdateNotification(int Id, Notification data, string fileName)
        {
            var ObjNotification = await _context.Notification.FirstOrDefaultAsync(n => n.Id == Id);
            if (ObjNotification != null)
            {
                ObjNotification.Code = data.Code;
                ObjNotification.IssuedBy = data.IssuedBy;
                if(fileName != null)
                {
                    ObjNotification.NotificationFile = fileName;
                }
                else
                {
                    ObjNotification.NotificationFile = data.NotificationFile;
                }
                ObjNotification.NotificationDate = data.NotificationDate;
                ObjNotification.Status = data.Status;
                ObjNotification.Description = data.Description;
                ObjNotification.UpdatedOn = DateTime.Now.Date;
                ObjNotification.UpdatedBy = data.UpdatedBy;

                //update the data to the database
                await _context.SaveChangesAsync();

            }
        }
    }
}
