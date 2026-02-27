using FluentValidation;
using HHSurvey.Models;
using System.Text.RegularExpressions;

public class HouseholdBasicProfileValidator
    : AbstractValidator<HouseholdBasicProfile>
{
    public HouseholdBasicProfileValidator(
        IXmlValidationRuleService ruleService)
    {
        // -------- Location --------
        RuleFor(x => x.District).NotEmpty();
        RuleFor(x => x.Block).NotEmpty();
        RuleFor(x => x.GramPanchayat).NotEmpty();
        RuleFor(x => x.RevenueVillage).NotEmpty();

        // -------- Head --------
        RuleFor(x => x.HeadOfTheHouseholdNameAsPerAadhar)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.HeadOfTheHouseholdGender)
            .Must(v => ruleService.GetAllowedValues("HeadOfTheHouseholdGender").Contains(v))
            .WithMessage("Invalid gender");

        RuleFor(x => x.AadharNo)
            .NotEmpty()
            .Matches(@"^\d{12}$")
            .WithMessage("Aadhar must be 12 digits");

        RuleFor(x => x.SocialCategory)
            .Must(v => ruleService.GetAllowedValues("SocialCategory").Contains(v))
            .WithMessage("Invalid social category");

        // -------- Bank --------
        RuleFor(x => x.BankAccountNumber)
            .NotEmpty()
            .Matches(@"^\d{9,18}$")
            .WithMessage("Invalid bank account number");

        RuleFor(x => x.IFSCcodeOrBranch)
            .NotEmpty()
            .Matches(@"^[a-zA-Z]{4}0[a-zA-Z0-9]{6}$")
            .WithMessage("Invalid IFSC code");

        // -------- Women --------
        //RuleFor(x => x.WomenMemberName).NotEmpty();

        // RuleFor(x => x.WomenMemberAge)
        //     .InclusiveBetween(16, 75)
        //     .WithMessage("Women member age must be 16–75");

        // RuleFor(x => x.WomenMemberMaritalStatus)
        //     .Must(v => ruleService.GetAllowedValues("MaritalStatus").Contains(v))
        //     .WithMessage("Invalid marital status");

        // RuleFor(x => x.WomenMemberRelationshipWithHead)
        //     .Must(v => ruleService.GetAllowedValues("RelationshipWithHead").Contains(v))
        //     .WithMessage("Invalid relationship with head");

        // -------- Household --------
        RuleFor(x => x.TotalFamilyMembers)
            .InclusiveBetween(1, 15);

     When(x => x.HasRationCard, () =>
{
    RuleFor(x => x.RationCardNumber)
        .NotEmpty()
        .Matches(@"^[a-zA-Z0-9]{11,12}$")
        .WithMessage("Invalid ration card number");
});

        RuleFor(x => x.DrinkingWaterSource)
            .Must(v => ruleService.GetAllowedValues("DrinkingWaterSource").Contains(v))
            .WithMessage("Invalid drinking water source");

        // -------- Metadata --------
        RuleFor(x => x.entryBy).NotEmpty();
    }
}
