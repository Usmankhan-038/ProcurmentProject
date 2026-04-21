namespace ProcurmentProject.Interfaces
{
    public interface IEmailService
    {
        public Task SendMail(string toEmail, string subject, string message);
    
    }
}
