namespace ecobony.application.Validator;

public class CreateHeaderValidator:AbstractValidator<CreateHeaderDto>
{
    public CreateHeaderValidator(IStringLocalizer<CreateHeaderDto> stringLocalizer)
    {
        RuleFor(a => a.Role)
            .NotEmpty().WithMessage(stringLocalizer["NotEmptyValidator"]);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        RuleFor(x => x.FomFile)
            .NotNull().WithMessage(stringLocalizer["RequiredValidator"])
            .NotEmpty().WithMessage(stringLocalizer["NotEmptyValidator"])
            .Must(file => file.Length > 0).WithMessage(stringLocalizer["NotEmptyValidator"])
            .Must(file => allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage(stringLocalizer["FileFormatNotAllowed"])
            .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage(stringLocalizer["FileSizeExceeded"]);
    }
}