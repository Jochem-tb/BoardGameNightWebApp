using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BGN.UI.Areas.Identity
{
    public class EmailSenderConsole : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Sending email to {email} with subject {subject}");
            return Task.CompletedTask;
        }
    }
}
