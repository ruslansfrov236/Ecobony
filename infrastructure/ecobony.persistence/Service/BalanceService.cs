
using ecobony.domain.Entities.Enum;

namespace ecobony.persistence.Service
{
    public class BalanceService(IBalanceReadRepository balanceRead,
                                IBalanceWriteRepository balanceWrite,
                                IBalanceTransferReadRepository balanceTransferRead,
                                IBalanceTransferWriteRepository balanceTransferWrite,
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
                throw new UnauthorizedAccessException("User not authenticated");

            var user = await userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found");

            if (!Guid.TryParse(bonusId, out _))
                throw new BadRequestException($"Invalid GUID format: '{bonusId}'");

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
                    throw new BadRequestException("Insufficient bonus points");


                bonusComunity.Score -= bonus;
                bonusComunity.PricePoint -= bonus;


                bonusComunityWriteRepository.Update(bonusComunity);
                await bonusComunityWriteRepository.SaveChangegesAsync();

                var balanceTransferCheck = await balanceTransferRead.GetSingleAsync(a => a.BalanceId == balance.Id && a.isDeleted == false);

                if (balanceTransferCheck is not null)
                {
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
                        LastUpdated = DateTime.UtcNow,
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
                throw new BadRequestException($"Invalid GUID format: '{id}'");
            var balance = await balanceRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == true) ?? throw new NotFoundException("Balance not found");

            balanceWrite.Delete(balance);
            await balanceWrite.SaveChangegesAsync();
            return true;

        }

        public async Task<List<BalanceTransfer>> GetAdminAll()
        => await balanceTransferRead.GetAll().ToListAsync();

        public async Task<BalanceTransfer> GetById(string id)
        => await balanceTransferRead.GetSingleAsync(a => a.BalanceId == Guid.Parse(id)) ?? throw new NotFoundException("Balance not found");

        public async Task<List<BalanceTransfer>> GetClientAll()
        {
            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("User not authenticated");

            var user = await userManager.FindByNameAsync(username) ?? throw new NotFoundException("User not found");
            var balance = await balanceRead.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted == false) ?? throw new NotFoundException("Balance not found");

            var balanceTransfer = await balanceTransferRead.GetFilter(a => a.BalanceId == balance.Id && a.isDeleted == false).ToListAsync();


            return balanceTransfer;
        }

        public async Task<bool> RestoreDelete(string id)
        {
            if (!Guid.TryParse(id, out _))
                throw new BadRequestException($"Invalid GUID format: '{id}'");
            var balance = await balanceRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == true) ?? throw new NotFoundException("Balance not found");

            balance.isDeleted = false;
            balanceWrite.Update(balance);
            await balanceWrite.SaveChangegesAsync();
            return true;
        }

        public async Task<bool> SoftDelete(string id)
        {
            if (!Guid.TryParse(id, out _))
                throw new BadRequestException($"Invalid GUID format: '{id}'");
            var balance = await balanceRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == false) ?? throw new NotFoundException("Balance not found");
            balance.isDeleted = true;
            balanceWrite.Update(balance);
            await balanceWrite.SaveChangegesAsync();
            return true;
        }
    }
}