using ecobony.domain.Dto_s.Authorize;
using ecobony.domain.Entities.Identity;

namespace ecobony.application.Services;

public interface IUserService
{
    Task<AppUser> CreateAsync(CreateUserDto_s model);
    Task<AppUser> UserDashboard();
    Task<bool>  UpdateRefreshToken (string refreshToken, AppUser user, DateTime refreshTokenDate ,int addOnAccessTokenDate);
    Task UpdatedPassword(string userId , string resetToken , string newPassword);
    Task<List<AppUser>> GetAllUsersAsync(int page, int size);
   
    int TotalUsersCount {get;}
    Task AssignRoleToUserAsync( string userId , string[] Roles);
    Task<string[]> GetRoleToUserAsync( string userIdOrName);
    Task<bool> AssignRoleDeleteUser(string userId);
}