using ecobony.application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.signair.Hubs
{
    public class UserHistoryHub(IUserHistoryReadRepository _read , IUserHistoryWriteRepository _write ):Hub
    {
    }
}
