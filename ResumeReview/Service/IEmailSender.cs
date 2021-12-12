using System.Threading.Tasks;

namespace ResumeReview.Service
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string ToEmail, string Subject, string Body);
    }
}