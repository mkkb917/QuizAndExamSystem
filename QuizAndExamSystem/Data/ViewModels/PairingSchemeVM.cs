using ExamSystem.Models;

namespace ExamSystem.Data.ViewModels
{
    public class PairingSchemeVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public List<PairingScheme> Scheme { get; set; }
    }
}
