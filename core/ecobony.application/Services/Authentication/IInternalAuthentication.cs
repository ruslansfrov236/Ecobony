namespace ecobony.application.Services.Authentication;

public interface IInternalAuthentication
{
    Task<Token> LoginAsync(string password , string usernameOrEmail ,int accessTokenLifeTime, bool isSave=false);
    Task<Token> RefreshTokenLoginAsync(string refreshToken);
}