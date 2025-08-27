using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Services
{
    public interface IUserTrackingService
    {
        Task<bool> Create(string path);
    }
}
