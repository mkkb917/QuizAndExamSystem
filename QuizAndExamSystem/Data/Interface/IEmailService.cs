namespace ExamSystem.Data.Interface
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string userEmail, string subject, string htmlMessage);
    }
}
