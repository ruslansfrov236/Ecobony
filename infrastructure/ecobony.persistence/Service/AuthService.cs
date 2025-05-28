using ecobony.signair.Services;

namespace ecobony.persistence.Service;

public class AuthService( 
 UserManager<AppUser>  _userManager,
 AuthHubService _authHub,
 ITokenService _tokenHandler,
 IConfiguration _configuration,
HttpClient _httpClient,
SignInManager<AppUser>  _signinManager,
  IUserService _userService,
 IMailService _mailService):IAuthService
{
  public async Task<Token> FacebookLoginAsync(string authToken ,int accessTokenLifeTime)
      { 
          string accessTokenResponse= await  _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["Facebook:Client_ID"]}&client_secret={_configuration["Facebook:Client_Secret"]}&grant_type=client_credentials");
        FacebookAccessToken_Dto_s? facebookAccessToken=  JsonSerializer.Deserialize<FacebookAccessToken_Dto_s>(accessTokenResponse);
        string userAccessTokenValidators=   await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessToken.AccessToken}");
        
         FacebookUserAccessTokenValidation? validation= 
                                      JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidators);


        if(validation?.Data?.IsValid !=null)
        {
                                        string userResponse= await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email ,name&access_token={authToken}");
                                      
                                                  FacebookUserInfoResponse? userInfoResponse=
                                                     JsonSerializer.Deserialize<FacebookUserInfoResponse>(userResponse);

      var info=  new UserLoginInfo("FACEBOOK", validation.Data.UserId , "FACEBOOK");

       AppUser? user=  await  _userManager.FindByLoginAsync(info.LoginProvider , info.ProviderKey);

       await _authHub.Connect();
       
   return   await  CreateUserExternalAsync(user , userInfoResponse.Email , userInfoResponse.Name , info ,accessTokenLifeTime);
      }
       throw new CustomUnauthorizedException("Invalid external authentication");
      }
      public async Task<Token> GoogleLoginAsync(string idToken ,int accessTokenLifeTime)
      {

  
           var settings=  new GoogleJsonWebSignature.ValidationSettings(){
            Audience=new List<string> { $"{_configuration["Google:Client_ID"]}"  }

        };

       var payload= await GoogleJsonWebSignature.ValidateAsync(idToken , settings);

       var info=  new UserLoginInfo("GOOGLE", payload.Subject , "GOOGLE");


       AppUser user=  await  _userManager.FindByLoginAsync(info.LoginProvider , info.ProviderKey);
       await _authHub.Connect();
         return   await  CreateUserExternalAsync(user , payload.Email , payload.Name , info ,accessTokenLifeTime);
      }

      public async  Task<Token> LoginAsync(string password , string usernameOrEmail , int accessTokenLifeTime, bool isSave=false)
      {
         AppUser user=  await   _userManager.FindByNameAsync(usernameOrEmail)
          ??   await _userManager.FindByEmailAsync(usernameOrEmail)
          ??   throw new CustomUnauthorizedException("The user did not verify the account");

         var result=   await  _signinManager.CheckPasswordSignInAsync(user , password ,isSave);
       if(result.Succeeded){
        Token token = await _tokenHandler.CreateAccessToken(accessTokenLifeTime , user);
        await  _userService.UpdateRefreshToken(token.RefreshToken, user , token.Expiration, 900);
                 
        await _authHub.Connect();
         return token;

       
       }

       throw new CustomUnauthorizedException("The user did not verify the account");
      }

      public async Task PasswordResetAsync(string email)
      {
       AppUser user= await _userManager.FindByEmailAsync(email);

       if(user!=null){
        string resetToken =await _userManager.GeneratePasswordResetTokenAsync(user);

        resetToken = WebUtility.UrlDecode(resetToken);

       await _mailService.SendPasswordResetMailAsync(email,  user.Id , resetToken);
       }
      }

      public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
      {
          AppUser? user =  _userManager.Users.FirstOrDefault(u=>u.RefreshToken==refreshToken);

          if(user!=null && user?.RefreshTokenEndDate>DateTime.UtcNow){

            Token token = await  _tokenHandler.CreateAccessToken(15, user);
            await _userService.UpdateRefreshToken(token.RefreshToken , user  , token.Expiration , 900);
            return token;
          }

          throw new CustomUnauthorizedException();
      }

    public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
{
    if (string.IsNullOrWhiteSpace(resetToken) || string.IsNullOrWhiteSpace(userId))
        return false;

    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
        return false;

    var decodedToken = WebUtility.UrlDecode(resetToken);
    var isValid = await _userManager.VerifyUserTokenAsync(
        user,
        _userManager.Options.Tokens.PasswordResetTokenProvider,
        "ResetPassword",
        decodedToken
    );

    return isValid;
}


     public async Task<Token> CreateUserExternalAsync(AppUser user , string email , string name , UserLoginInfo info , int accessTokenLifeTime){
      bool result= user!=null;
       if(user==null){
          user = await _userManager.FindByEmailAsync(email);
          if(user==null){
            user= new (){
                Id=Guid.NewGuid().ToString(),
                Email=email,
                UserName=email,
                NameSurname=name
            };

           var identityResult= await _userManager.CreateAsync(user);

           result=identityResult.Succeeded;
          }
          
       }

             if(result){
                  await  _userManager.AddLoginAsync(user , info);
                  Token token = await _tokenHandler.CreateAccessToken(accessTokenLifeTime , user);
                  await  _userService.UpdateRefreshToken(token.RefreshToken, user , token.Expiration, 900 );
                  return token;

             }
               throw new CustomUnauthorizedException("Invalid external authentication");
        } 

        
    
        }
   