using ecobony.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.persistence.Repository
{
    public class UserHistoryReadRepository(AppDbContext _context):ReadRepository<UserHistory>(_context) , IUserHistoryReadRepository
    {
    }
}
