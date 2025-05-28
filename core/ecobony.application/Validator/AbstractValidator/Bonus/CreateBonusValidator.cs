namespace ecobony.application.Validator;

public class CreateBonusValidator:AbstractValidator<CreateBonusCommunityDto_s>
{
    public CreateBonusValidator(IStringLocalizer<CreateBonusCommunityDto_s> stringLocalizer)
    {
        RuleFor(x => x.Score) 
            .GreaterThan(0m).WithMessage(stringLocalizer["GreaterThanValidator"]); 


    }
}