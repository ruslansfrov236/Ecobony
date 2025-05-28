public class CreateLanguageValidator : AbstractValidator<CreateLanguageDto_s>
{
    public CreateLanguageValidator()
    {
        RuleFor(a => a.IsoCode)
            .NotNull().WithMessage(ValidationLanguageStore.Get("ISOCodeCannotBeNull"))
            .NotEmpty().WithMessage(ValidationLanguageStore.Get("ISOCodeCannotBeEmpty"))
            .Length(5, 7).WithMessage(ValidationLanguageStore.Get("ISOCodeLength"))
            .Must(BeAValidCulture).WithMessage(ValidationLanguageStore.Get("InvalidISOCode"));

        RuleFor(a => a.Name)
            .NotNull().WithMessage(ValidationLanguageStore.Get("CultureIsRequired"))
            .NotEmpty().WithMessage(ValidationLanguageStore.Get("CultureCannotBeEmpty"))
            .Length(5, 20).WithMessage(ValidationLanguageStore.Get("CultureLength"));

        RuleFor(a => a.Key)
            .NotNull().WithMessage(ValidationLanguageStore.Get("KeyCannotBeNull"))
            .MaximumLength(3).WithMessage(ValidationLanguageStore.Get("KeyMaxLength"));

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        RuleFor(x => x.FormFile)
            .NotNull().WithMessage(ValidationLanguageStore.Get("FileIsRequired"))
            .NotEmpty().WithMessage(ValidationLanguageStore.Get("FileCannotBeEmpty"))
            .Must(file => file.Length > 0).WithMessage(ValidationLanguageStore.Get("FileCannotBeEmpty"))
            .Must(file => allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(ValidationLanguageStore.Get("FileFormatNotAllowed"))
            .Must(file => file.Length <= 5 * 1024 * 1024)
            .WithMessage(ValidationLanguageStore.Get("FileSizeExceeded"));
    }

    private bool BeAValidCulture(string isoCode)
    {
        return CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Any(c => c.Name.Equals(isoCode, StringComparison.OrdinalIgnoreCase));
    }
}