using ecobony.domain.Dto_s.Authorize;

namespace ecobony.application.Services;

public interface IOTPService
{
    Task SaveVerificationCodeAsync(string userId, string code);
    Task VerificationCodeAsync(CreateVerifactionDto_s model);
    Task Remove(string id);

    Task<bool> EmailConfiremed(string userId);
    string GenerateVerificationCode(int length = 6);
}