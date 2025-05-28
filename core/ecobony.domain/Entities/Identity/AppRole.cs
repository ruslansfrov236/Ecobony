
namespace ecobony.domain.Entities.Identity;

public class AppRole:IdentityRole<string>
{
    public RoleModel RoleModel { get; set; }
}