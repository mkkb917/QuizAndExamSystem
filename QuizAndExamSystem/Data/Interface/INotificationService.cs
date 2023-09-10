using ExamSystem.Data.Base;
using ExamSystem.Data.ViewModels;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface INotificationService: IEntityBaseRepository<Notification>
    {
        Task<Notification> GetNotificationById(int id);
        Task<List<Notification>> GetAllNotificationByUser(string Id);
        Task<List<Notification>> GetAllNotification();
        Task AddNewNotification(Notification data, IFormFileCollection files);
        Task UpdateNotification(int Id, Notification data, string fileName);
        Task DeleteNotification(int id);

    }
}
