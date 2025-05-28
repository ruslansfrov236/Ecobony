

namespace ecobony.application.Services;

public interface ITokenService
{
    Task<Token> CreateAccessToken(int second, AppUser appUser);
    string DecryptRefreshToken(string encryptedToken);
}