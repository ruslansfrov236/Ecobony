using ecobony.domain.Dto_s.Authorize;

namespace ecobony.persistence.Service;

public class RoleService(RoleManager<AppRole> _roleManager):IRoleService
{
    public async Task<List<AppRole>> GetAll()
        => await _roleManager.Roles.ToListAsync();

    public async Task<AppRole> GetById(string id)
    {
      
            if (!Guid.TryParse(id, out _))
                throw new BadRequestException($"Invalid GUID format: '{id}'");
            var role = await _roleManager.FindByIdAsync(id) ?? throw new NotFoundException();

            return role;
    }

    public async Task<bool> Create(CreateRolesDto_s model)
    {
       if(await _roleManager.Roles.AnyAsync(a=>a.Name==model.Name))
           throw new BadRequestException("Replat values name ");
       if(await _roleManager.Roles.AnyAsync(a=>a.RoleModel==model.RoleModel))
           throw new BadRequestException("Replat values  role model  ");

       var role = new AppRole()
       {
           Name = model.Name,
           ConcurrencyStamp = model.Name.ToUpper(),
           RoleModel = model.RoleModel
       };
       await _roleManager.CreateAsync(role);
       return true;
    }

    public async Task<bool> Update(UpdateRolesDto_s model)
    {
        if (!Guid.TryParse(model.Id, out _))
            throw new BadRequestException($"Invalid GUID format: '{model.Id}'");
        var role = await _roleManager.FindByIdAsync(model.Id) ?? throw new NotFoundException();

        role.Name = model.Name;
        role.ConcurrencyStamp = model.Name.ToUpper();
        role.RoleModel = model.RoleModel;

        await _roleManager.UpdateAsync(role);
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var role = await _roleManager.FindByIdAsync(id) ?? throw new NotFoundException();

        await _roleManager.DeleteAsync(role);
        return true;
    }
}