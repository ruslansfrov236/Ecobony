using Microsoft.Extensions.Localization;

namespace ecobony.application.Validator;

public class CreateLanguageValidator : AbstractValidator<CreateLanguageDto_s>
{
    private readonly IStringLocalizer _stringLocalizer;

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private static readonly string[] AllCultureNames = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                                                  .Select(c => c.Name)
                                                                  .ToArray();

    public CreateLanguageValidator(IStringLocalizer stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;

        // ISOCode qaydaları
        RuleFor(a => a.IsoCode)
            .NotNull().WithMessage(_stringLocalizer["ISOCodeCannotBeNull"])
            .NotEmpty().WithMessage(_stringLocalizer["ISOCodeCannotBeEmpty"])
            .Length(5, 7).WithMessage(_stringLocalizer["ISOCodeLength"])
            .Must(BeAValidCulture).WithMessage(_stringLocalizer["InvalidISOCode"]);

        // Name qaydaları
        RuleFor(a => a.Name)
            .NotNull().WithMessage(_stringLocalizer["RequiredValidator"])
            .NotEmpty().WithMessage(_stringLocalizer["NotEmptyValidator"])
            .Length(5, 20).WithMessage(_stringLocalizer["CultureLength"]);

        // Key qaydaları
        RuleFor(a => a.Key)
            .NotNull().WithMessage(_stringLocalizer["KeyCannotBeNull"])
            .MaximumLength(3).WithMessage(_stringLocalizer["KeyMaxLength"]);

        // Fayl qaydaları
        RuleFor(x => x.FormFile)
            .Must(file => file != null)
            
                .WithMessage(_stringLocalizer["FileIsRequired"])
            .Must(file => file != null && file.Length > 0)
                .WithMessage(_stringLocalizer["FileCannotBeEmpty"])
            .Must(file => file != null && AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage(_stringLocalizer["FileFormatNotAllowed"])
            .Must(file => file != null && file.Length <= 5 * 1024 * 1024)
                .WithMessage(_stringLocalizer["FileSizeExceeded"]);
    }

    private bool BeAValidCulture(string isoCode)
    {
        return !string.IsNullOrWhiteSpace(isoCode) &&
               AllCultureNames.Contains(isoCode, StringComparer.OrdinalIgnoreCase);
    }
}
