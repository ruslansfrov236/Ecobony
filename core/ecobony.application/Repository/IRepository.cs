using ecobony.domain.Entities.Comunity;

namespace ecobony.application.Repository;

public class IRepository<T> where T:BaseEntity
{
    private DbSet<T> Table { get;  }
}