namespace ecobony.application.Validator;

public class UpdateBonusValidator:AbstractValidator<UpdateBonusCommunityDto_s>
{
    public UpdateBonusValidator(IStringLocalizer<UpdateBonusCommunityDto_s> localizer)
    {
        RuleFor(x => x.Score) 
            .GreaterThan(0m).WithMessage(localizer["GreaterThanValidator"]); 
    }
}