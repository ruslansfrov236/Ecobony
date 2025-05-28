namespace ecobony.application.Services.Authentication;

public interface IExternalAuthentication
{
    Task<Token> FacebookLoginAsync( string authToken , int accessTokenLifeTime);
    Task<Token> GoogleLoginAsync(string idToken ,int accessTokenLifeTime);
}