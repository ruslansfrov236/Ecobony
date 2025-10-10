 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.persistence.Repository
{
    public class LogOptionsReadRepository (AppDbContext context): ReadRepository<LogOptions>(context), ILogOptionReadRepository
    {
    }
}
