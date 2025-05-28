using ecobony.application.Services.Authentication;

namespace ecobony.application.Services;

public interface IAuthService:IExternalAuthentication , IInternalAuthentication
{
    Task  PasswordResetAsync(string email);
    Task <bool> VerifyResetTokenAsync(string resetToken , string userId);
}