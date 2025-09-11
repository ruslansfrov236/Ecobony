using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.persistence.Service
{
    public class LanguageJsonService(IStringLocalizer stringLocalizer) : ILanguageJsonService
    {
        public string LanguageStrongJson(string key)
        {
            if (key is null) throw new NotFoundException(stringLocalizer.GetString("NotFound"));
            var message = stringLocalizer.GetString(key) ?? throw new NotFoundException(stringLocalizer.GetString("NotFound"));
            return message;
        }
    }
}
