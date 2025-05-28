using ecobony.application.Services;
using ecobony.application.Validator;
using ecobony.domain.Dto_s.Authorize;
using ecobony.domain.Entities.Enum;

namespace ecobony.persistence.Service;

public class UserService(
    RoleManager<AppRole> _roleManager,
    UserManager<AppUser> _userManager,
    IHttpContextAccessor _contextAccessor,
    IOTPService _otpService
) : IUserService
{
    public async Task<AppUser> CreateAsync(CreateUserDto_s model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null) throw new BadRequestException("Email already exists.");


        var isboolReplayUserName = await _userManager.FindByNameAsync(model.UserName);
        if (isboolReplayUserName != null) throw new BadRequestException("Values is replay username");


        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),

            UserName = model.UserName,
            Email = model.Email
        };


        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
          
            foreach (var error in result.Errors)
                throw new BadRequestException(error.Description);


        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            var code = _otpService.GenerateVerificationCode();


            await   _otpService.SaveVerificationCodeAsync(user.Id, code);
            // await _emailService.SendVerificationCodeEmailAsync(user.Email, code, user.UserName);
            await _userManager.AddToRoleAsync(user, nameof(RoleModel.Manager));
        }

        return user;
    }

    public async Task<AppUser> UserDashboard()
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) throw new NotFoundException();
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found.");

        return user;
    }

    public async Task<bool> UpdateRefreshToken(string refreshToken, AppUser user, DateTime refreshTokenDate,
        int addOnAccessTokenDate)
    {
        if (user == null) throw new NotFoundException("User not found");

        user.RefreshToken = refreshToken;
        user.RefreshTokenEndDate = refreshTokenDate.ToUniversalTime().AddSeconds(addOnAccessTokenDate);

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }


    public async Task UpdatedPassword(string userId, string resetToken, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded) await _userManager.UpdateSecurityStampAsync(user);
        }
    }

    public async Task<List<AppUser>> GetAllUsersAsync(int page, int size)
    {
        var user = await _userManager.Users.Skip(page * size).Take(size).ToListAsync();

        return user;
    }

    public int TotalUsersCount => _userManager.Users.Count();

    public async Task AssignRoleToUserAsync(string userId, string[] Roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, Roles);
        }
    }

    public async Task<string[]> GetRoleToUserAsync(string userIdOrName)
    {
        var user = await _userManager.FindByIdAsync(userIdOrName)
                   ?? await _userManager.FindByNameAsync(userIdOrName);

        if (user == null)
            throw new NotFoundException("User not found");

        return (await _userManager.GetRolesAsync(user)).ToArray();
    }


    public async Task<bool> AssignRoleDeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);


            foreach (var roleName in userRoles) await _userManager.RemoveFromRoleAsync(user, roleName);

            return true;
        }

        return false;
    }
}
