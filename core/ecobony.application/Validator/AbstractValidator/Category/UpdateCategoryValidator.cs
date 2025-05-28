namespace ecobony.application.Validator;

public class UpdateCategoryValidator:AbstractValidator<UpdateCategoryDto_s>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Pointy)
            .GreaterThanOrEqualTo(0).WithMessage("Pointy must be greater than or equal to 0.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        RuleFor(x => x.FormFile)
            .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
            .Must(file => allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage("Only .jpg, .jpeg, .png, .gif formats are allowed.")
            .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("File size must be less than 5MB.");
    }
}