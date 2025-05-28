

using ecobony.application.Validator;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ecobony.infrastructur.Application;

public class OTPService(IDistributedCache _cache, UserManager<AppUser> _userManager):IOTPService
{
   

    public async Task SaveVerificationCodeAsync(string userId, string code)
    {
        var cacheKey = $"VerificationCode_{userId}";
        var encryptedCode = SecurityHelper.EncryptVerificationCode(code, userId);

        var verificationCode = new VerificationCode(userId, encryptedCode, DateTime.UtcNow.AddMinutes(60));

        var serializedData = JsonSerializer.Serialize(verificationCode);

        var cacheEntryOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

        await _cache.SetStringAsync(cacheKey, serializedData, cacheEntryOptions);
    }

    public async Task VerificationCodeAsync(CreateVerifactionDto_s model)
    {
        var cacheKey = $"VerificationCode_{model.UserId}";

        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (string.IsNullOrEmpty(cachedData))
            throw new Exception("The verification code is incorrect or does not exist.");

        var cachedCode = JsonSerializer.Deserialize<VerificationCode>(cachedData);
        if (cachedCode == null || cachedCode.Code != SecurityHelper.EncryptVerificationCode(model.Code, model.UserId))
            throw new BadRequestException("The verification code is incorrect.");

        if (DateTime.UtcNow > cachedCode.ExpiryTime)
        {
            await Remove(model.UserId);
            throw new BadRequestException("The verification code has expired.");
        }

        var isConfirmed = await EmailConfiremed(cachedCode.UserId);
        if (isConfirmed) await Remove(cachedCode.UserId);
    }


    public Task Remove(string userId)
    {
        var cacheKey = $"VerificationCode_{userId}";
        _cache.Remove(cacheKey);
        return Task.CompletedTask;
    }

    public async Task<bool> EmailConfiremed(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new NotFoundException("User not found");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);


        return true;
    }

    public string GenerateVerificationCode(int length = 6)
    {
        var random = new Random();
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}