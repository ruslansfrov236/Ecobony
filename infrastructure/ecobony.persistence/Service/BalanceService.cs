
using ecobony.domain.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace ecobony.persistence.Service
{
    public class BalanceService(IBalanceReadRepository balanceRead,
                                IBalanceWriteRepository balanceWrite,
                                ILanguageJsonService  languageJsonService,
                                IBalanceTransferReadRepository balanceTransferRead,
                                IBalanceTransferWriteRepository balanceTransferWrite,
                                RoleManager<AppRole> roleManager,
                                IHttpContextAccessor _contextAccessor,
                                IBonusReadRepository bonusReadRepository,
                                IBonusWriteRepository bonusWriteRepository,
                                IBonusComunityReadRepository bonusComunityReadRepository,
                                IBonusComunityWriteRepository bonusComunityWriteRepository,
                                UserManager<AppUser> userManager
                                ) : IBalanceService
    {

        public async Task<bool> Create(string bonusId, decimal bonus)
        {

            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new CustomUnauthorizedException(languageJsonService.LanguageStrongJson("Unauthorized"));

            var user = await userManager.FindByNameAsync(username) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("UserNotFound"));

            var idempotencyKey = _contextAccessor?.HttpContext?.Request.Headers["Idempotency-Key"].FirstOrDefault();

            if (string.IsNullOrEmpty(idempotencyKey))
            {
                throw new BadRequestException(languageJsonService.LanguageStrongJson("IdempotencyKeyRequired"));

            }
            if (!Guid.TryParse(bonusId, out _))
                throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));

            var balance = await balanceRead.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted == false);


            if (balance is null)
            {
                var newBalance = new Balance()
                {
                    UserId = user.Id,

                };
                await balanceWrite.AddAsync(balance);
                await balanceWrite.SaveChangegesAsync();
            }
            else
            {
                var bonusValue = await bonusReadRepository.
           GetSingleAsync(a => a.UserId == balance.UserId && a.Id == Guid.Parse(bonusId) && a.isDeleted == false) ?? throw new NotFoundException("Bonus not found");

                var bonusComunity = await bonusComunityReadRepository.GetSingleAsync(a => a.BonusId == bonusValue.Id && a.isDeleted == false) ?? throw new NotFoundException("Bonus not found");
                if (bonusComunity.Score < bonus && bonusComunity.PricePoint < bonus && bonusComunity.Score < 0)
                    throw new BadRequestException(languageJsonService.LanguageStrongJson("InsufficientBonusPoints"));


                bonusComunity.Score -= bonus;
                bonusComunity.PricePoint -= bonus;


                bonusComunityWriteRepository.Update(bonusComunity);
                await bonusComunityWriteRepository.SaveChangegesAsync();

                var balanceTransferCheck = await balanceTransferRead.GetSingleAsync(a => a.BalanceId == balance.Id && a.isDeleted == false);

                if (balanceTransferCheck is not null)
                {
                    balanceTransferCheck.IdempotencyKey = idempotencyKey;
                    balanceTransferCheck.Amount += bonus;
                    balanceTransferWrite.Update(balanceTransferCheck);
                    await balanceTransferWrite.SaveChangegesAsync();
                }
                else
                {
                    var balanceTransfer = new BalanceTransfer

                    {
                        BalanceId = balance.Id,
                        Valyuta = Valyuta.AZN,
                        LastUpdated = DateTime.UtcNow.ToLocalTime(),
                        IdempotencyKey= idempotencyKey, 
                        Amount = bonus
                    };

                    await balanceTransferWrite.AddAsync(balanceTransfer);
                    await balanceTransferWrite.SaveChangegesAsync();

                }

            }

            return true;
        }

        public async Task<bool> Delete(string id)
        {
            if (!Guid.TryParse(id, out _))
                throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
            var balance = await balanceRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == false) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

            balanceWrite.Delete(balance);
            await balanceWrite.SaveChangegesAsync();
            return true;

        }

        public async Task<List<BalanceTransfer>> GetAdminAll()
        => await balanceTransferRead.GetAll().ToListAsync();

        public async Task<BalanceTransfer> GetById(string id)
        {
           
            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                throw new CustomUnauthorizedException(languageJsonService.LanguageStrongJson("Unauthorized"));

            AppUser? user = await userManager.FindByNameAsync(username)
                ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("UserNotFound"));

            var userRoles = await userManager.GetRolesAsync(user)
                ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("RoleNotFound"));
            var bs = await balanceRead.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted==false) ??
                throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));
            
            BalanceTransfer? balance = null;

            foreach (var role in userRoles)
            {
                if (role == RoleModel.Admin.ToString() || role ==RoleModel.Manager.ToString())
                {
                    // Admin bütün balansları görə bilər
                    balance = await balanceTransferRead.GetByIdAsync(id);
                    break;
                }
                else if (role == RoleModel.User.ToString())
                {
                    // İstifadəçi yalnız öz balansını görə bilər
                    balance = await balanceTransferRead.GetSingleAsync(a =>
                        a.Id == Guid.Parse(id) &&
                        a.BalanceId==bs.Id &&
                        !a.isDeleted);
                    break;
                }
               
            }

            if (balance == null)
                throw new NotFoundException(languageJsonService.LanguageStrongJson("InsufficientBalance"));

            return balance;
        }


        public async Task<List<BalanceTransfer>> GetClientAll()
        {
            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new CustomUnauthorizedException(languageJsonService.LanguageStrongJson("Unauthorized"));

            var user = await userManager.FindByNameAsync(username) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("UserNotFound"));
            var balance = await balanceRead.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted == false) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

            var balanceTransfer = await balanceTransferRead.GetFilter(a => a.BalanceId == balance.Id && a.isDeleted == false).ToListAsync();


            return balanceTransfer;
        }

        public async Task<bool> RestoreDelete(string id)
        {
            if (!Guid.TryParse(id, out _))
                throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
            var balance = await balanceRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == true) ?? throw new NotFoundException(languageJsonService.LanguageStrongJson("NotFound"));

            balance.isDeleted = false;
            balanceWrite.Update(balance);
            await balanceWrite.SaveChangegesAsync();
            return true;
        }

        public async Task<bool> SoftDelete(string id)
        {
            if (!Guid.TryParse(id, out _))
                throw new BadRequestException(languageJsonService.LanguageStrongJson("InvalidGuid"));
            var balance = await balanceRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == false) ?? throw new NotFoundException("Balance not found");
            balance.isDeleted = true;
            balanceWrite.Update(balance);
            await balanceWrite.SaveChangegesAsync();
            return true;
        }
    }
}