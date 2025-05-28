namespace ecobony.application.Validator;

public class UpdateCategoryTranslationValidator:AbstractValidator<UpdateCategoryTranslationDto_s>
{
    public UpdateCategoryTranslationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}