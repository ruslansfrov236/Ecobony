
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ecobony.application.Validator;

namespace ecobony.infrastructur.Application;

public class TokenService( IConfiguration _configuration,
  UserManager<AppUser> _userManager
    ):ITokenService
{
  public async Task<Token> CreateAccessToken(int second, AppUser appUser)
    {
        var token = new Token();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        token.Expiration = DateTime.UtcNow.AddSeconds(second);

        var securityToken = new JwtSecurityToken(
            audience: _configuration["Token:Audience"],
            issuer: _configuration["Token:Issuer"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials,
            claims: new List<Claim> { new(ClaimTypes.Name, appUser.UserName) }
        );

        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);

       
        var encryptedRefreshToken = await GetEncryptedRefreshTokenFromDBAsync(appUser.Id);

     
        if (!string.IsNullOrEmpty(encryptedRefreshToken))
            token.RefreshToken = DecryptRefreshToken(encryptedRefreshToken);
        else
            token.RefreshToken = GenerateRefreshToken();

        return token;
    }

    public string DecryptRefreshToken(string encryptedToken)
    {
        var cipherText = Convert.FromBase64String(encryptedToken);

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = GetEncryptionKey();

           
            aesAlg.IV = cipherText.Take(16).ToArray();

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new(cipherText, 16, cipherText.Length - 16))
            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new(csDecrypt))
            {
                return srDecrypt.ReadToEnd(); 
            }
        }
    }

    private byte[] GetEncryptionKey()
    {
        var key = _configuration["Token:EncryptionKey"];
        var keyBytes = Encoding.UTF8.GetBytes(key);

        if (keyBytes.Length != 32)
            throw new ArgumentException("EncryptionKey must be exactly 32 bytes long for AES-256.");

        return keyBytes;
    }

    private async Task<string> GetEncryptedRefreshTokenFromDBAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User is not found");

        if (string.IsNullOrEmpty(user.RefreshToken))
            throw new NotFoundException("Refresh token is not found");

        return user.RefreshToken;
    }


    private string GenerateRefreshToken()
    {
        var number = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(number);

        return Convert.ToBase64String(number);
    }
}