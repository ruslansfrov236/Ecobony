
namespace ecobony.persistence;

public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}