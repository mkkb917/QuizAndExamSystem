using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class UserRolesAndPermissionsVM
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }
}
