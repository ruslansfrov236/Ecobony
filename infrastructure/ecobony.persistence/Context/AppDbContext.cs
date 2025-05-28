
namespace ecobony.persistence.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options):IdentityDbContext<AppUser , AppRole, string>(options)
{
    public DbSet<WasteSortingMistakeTranslation> WasteSortingMistakesTranslations { get; set; }
    public DbSet<SortingActionTranslation> SortingActionsTranslations { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    public DbSet<WasteSortingMistake> WasteSortingMistakes { get; set; }
   
    public DbSet<SortingAction> SortingActions { get; set; } 
    public DbSet<BonusComunity> BonusComunities { get; set; }
    public DbSet<WasteProcess> WasteProcesses { get; set; }

    public DbSet<BalanceTransfer> BalanceTransfers { get; set; }
    public DbSet<Balance> Balances { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Header>  Headers { get; set; }
    public DbSet<Bonus> Bonus { get; set; }
    public DbSet<Waste> Wastes { get; set; }

  
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var datas = ChangeTracker
            .Entries<BaseEntity>();
        foreach (var data in datas)
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreateAt = DateTime.Now,
                EntityState.Modified => data.Entity.UpdateAt = DateTime.Now,
                _ => DateTime.UtcNow
            };
        return await base.SaveChangesAsync(cancellationToken);
    }
}