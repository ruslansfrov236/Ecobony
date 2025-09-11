using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.domain.Entities.Enum
{
    public enum ValidationMessageKey
    {
        RequiredValidator,
        NotEmptyValidator,
        MaximumLengthValidator,
        GreaterThanValidator,
        ISOCodeCannotBeNull,
        ISOCodeCannotBeEmpty,
        ISOCodeLength,
        InvalidISOCode,
        CultureIsRequired,
        CultureCannotBeEmpty,
        CultureLength,
        KeyCannotBeNull,
        KeyMaxLength,
        FileIsRequired,
        FileCannotBeEmpty,
        FileFormatNotAllowed,
        FileSizeExceeded,
        NotNullValidator,
        LengthValidator
    }
}
