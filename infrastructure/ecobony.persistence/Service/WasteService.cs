using Microsoft.AspNetCore.Http;

namespace ecobony.persistence.Service;

public class WasteService(
    IWasteReadRepository _wasteRead,
    IWasteWriteRepository _wasteWrite,
    ICategoryReadRepository _categoryRead,
    IHttpContextAccessor _contextAccessor,
    UserManager<AppUser> _userManager):IWasteService
{
    public async Task<List<Waste>> GetClientAll()
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) throw new NotFoundException();
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found.");

        var waste = await _wasteRead.GetAll().Where(a => a.UserId == user.Id).ToListAsync();
        return waste;
    }

    public async Task<List<Waste>> GetAdminAll()
     => await _wasteRead.GetAll().ToListAsync();


    public async Task<Waste> GetById(string id)
    { if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var waste = await _wasteRead.GetByIdAsync(id) ?? throw new NotFoundException();
        return waste;
    }

    public async Task<List<Waste>> GetWasteTrash()
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) throw new NotFoundException();
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found.");

        var waste = await _wasteRead.GetFilter(a => a.isDeleted == true && a.UserId == user.Id).ToListAsync();
        return waste;
    }

    public async Task<List<Waste>> GetFilterWasteCategory(string categoryId)
    {
        if (!Guid.TryParse(categoryId, out _))
            throw new BadRequestException($"Invalid GUID format: '{categoryId}'");
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) throw new NotFoundException();
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found.");

        var waste = await _wasteRead
            .GetFilter(a => a.CategoryId == Guid.Parse(categoryId) && a.UserId == user.Id && a.isDeleted == false)
            .Include(a=>a.Category)
            .ToListAsync();

        return waste;
    }

    public async Task<bool> Create(CreateWasteDto_s model)
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) throw new NotFoundException();
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found.");
         
         var category = await _categoryRead.GetSingleAsync(a => a.Id == model.CategoryId && a.isDeleted==false) ?? throw new NotFoundException("Category not found.");
       
          var totalBonus = (category.Pointy* (model.Weight/1000));
       
        Waste waste = new Waste()
        {
            Title = model.Title,
            UserId = user.Id,
            CategoryId = category.Id,
            Station = model.Station,
            Weight = model.Weight,
            Score = totalBonus,
        };

        await _wasteWrite.AddAsync(waste);
        await _wasteWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Update(UpdateWasteDto_s model)
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username)) throw new NotFoundException();
        AppUser user = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found.");

        if (!Guid.TryParse(model.Id, out _))
            throw new BadRequestException($"Invalid GUID format: '{model.Id}'");
        var waste = await _wasteRead.GetSingleAsync(a=>a.Id==Guid.Parse(model.Id)&& a.UserId==user.Id) ?? throw new NotFoundException();
            var category = await _categoryRead.GetSingleAsync(a => a.Id == model.CategoryId && a.isDeleted==false) ?? throw new NotFoundException("Category not found.");
       
          var totalBonus = (category.Pointy* (model.Weight/1000));
          var totalPrice = (category.PricePoint * (model.Weight / 1000));
       
      
        waste.Title = model.Title;
        waste.UserId = user.Id;
        waste.CategoryId = category.Id;
        waste.Station = model.Station;
        waste.Weight = model.Weight;
        waste.Score = totalBonus;
        waste.PricePoint = totalPrice;
        
        _wasteWrite.Update(waste);
        await _wasteWrite.SaveChangegesAsync();
        return true;        
    }

    public async Task<bool> SoftDelete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var waste = await _wasteRead.GetSingleAsync(a=> a.Id== Guid.Parse(id) && a.isDeleted==false) ?? throw new NotFoundException();
        waste.isDeleted = true;
        _wasteWrite.Update(waste);
        await _wasteWrite.SaveChangegesAsync();
        return true;

    }

    public async Task<bool> Restore(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var waste = await _wasteRead.GetSingleAsync(a=> a.Id== Guid.Parse(id) && a.isDeleted==true) ?? throw new NotFoundException();
        waste.isDeleted=false;
        _wasteWrite.Update(waste);
        await _wasteWrite.SaveChangegesAsync();
        return true;
    }

    public async Task<bool> Delete(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new BadRequestException($"Invalid GUID format: '{id}'");
        var waste = await _wasteRead.GetSingleAsync(a=> a.Id== Guid.Parse(id) && a.isDeleted==true) ?? throw new NotFoundException();
        _wasteWrite.Delete(waste);
        await _wasteWrite.SaveChangegesAsync();
        return true;

    }
}