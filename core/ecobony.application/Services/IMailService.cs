namespace ecobony.application;

public interface IMailService
{
    
    Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
    Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
    Task SendVerificationCodeEmailAsync(string to, string code, string name);
    Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
    Task SendResponse(string to,  string name);
}