using FluentValidation;
using HHSurvey.Models;

public class HouseholdOccupationAndLandValidator
    : AbstractValidator<HouseholdOccupationAndLand>
{
    public HouseholdOccupationAndLandValidator(
        IXmlValidationRuleService ruleService)
    {
        // -------- PRIMARY OCCUPATION --------
        RuleFor(x => x.PrimaryOccupationOfTheFamily)
            .NotEmpty()
            .Must(value =>
                value.InIgnoreCase(
                    ruleService.GetAllowedValues("PrimaryOccupation").ToArray()))
            .WithMessage("Invalid Primary Occupation");

        RuleFor(x => x.OtherPrimaryOccupationDetails)
            .NotEmpty()
            .When(x =>
                x.PrimaryOccupationOfTheFamily != null &&
                x.PrimaryOccupationOfTheFamily.Equals(
                    "Other",
                    StringComparison.OrdinalIgnoreCase))
            .WithMessage("Other occupation details required");

        // -------- FRA --------
        // RuleFor(x => x.FRAClaimantStatus)
        //     .NotEmpty()
        //     .Must(value =>
        //         value.InIgnoreCase(
        //             ruleService.GetAllowedValues("FRAClaimantStatus").ToArray()))
        //     .WithMessage("Invalid FRA claimant status");

        // RuleFor(x => x.FRA_LandAmountInAcres)
        //     .InclusiveBetween(0.5m, 5m)
        //     .When(x =>
        //         x.FRAClaimantStatus != null &&
        //         x.FRAClaimantStatus.Equals(
        //             "FRA Claimant",
        //             StringComparison.OrdinalIgnoreCase));

        // RuleFor(x => x.FRA_LandAmountInAcres)
        //     .Null()
        //     .When(x =>
        //         x.FRAClaimantStatus != null &&
        //         x.FRAClaimantStatus.Equals(
        //             "Not a FRA Claimant",
        //             StringComparison.OrdinalIgnoreCase));

        // -------- PRIVATE LAND --------
        RuleFor(x => x.ApproximatePrivateLandHolding)
            .NotEmpty()
            .Must(value =>
                value.InIgnoreCase(
                    ruleService.GetAllowedValues("ApproximatePrivateLandHolding").ToArray()))
            .WithMessage("Invalid private land holding");

        // -------- LIVESTOCK (Multi Select) --------
        // RuleFor(x => x.InvolvedInLivestockActivity)
        //     .Must(value =>
        //     {
        //         if (string.IsNullOrWhiteSpace(value))
        //             return true;

        //         var allowed =
        //             ruleService.GetAllowedValues("LivestockActivity").ToArray();

        //         var selected = value.Split(',')
        //                             .Select(v => v.Trim());

        //         return selected.All(s => s.InIgnoreCase(allowed));
        //     })
        //     .WithMessage("Invalid livestock activity");
    }
}
