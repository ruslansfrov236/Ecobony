namespace ecobony.application.Validator;

public class CreateHeaderTranslationValidator:AbstractValidator<CreateHeaderTranslationDto_s>
{
    public CreateHeaderTranslationValidator(IStringLocalizer<CreateBonusCommunityDto_s> stringLocalizer)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(stringLocalizer["NotEmptyValidator"])
            .MaximumLength(100).WithMessage(stringLocalizer["MaximumLengthValidator"]);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(stringLocalizer["NotEmptyValidator"])
            .MaximumLength(500).WithMessage(stringLocalizer["MaximumLengthValidator"]);
    }
}