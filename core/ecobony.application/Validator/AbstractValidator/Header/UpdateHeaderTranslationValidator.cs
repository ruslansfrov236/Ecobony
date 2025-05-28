namespace ecobony.application.Validator;

public class UpdateHeaderTranslationValidator:AbstractValidator<UpdateHeaderTranslationDto_s>
{
    public UpdateHeaderTranslationValidator(IStringLocalizer<UpdateHeaderTranslationDto_s> stringLocalizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(stringLocalizer["NotEmptyValidator"])
            .MaximumLength(100).WithMessage(stringLocalizer["MaximumLengthValidator"]);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(stringLocalizer["NotEmptyValidator"])
            .MaximumLength(500).WithMessage(stringLocalizer["MaximumLengthValidato"]);
    }
}