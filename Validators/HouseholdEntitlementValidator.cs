using FluentValidation;
using HHSurvey.Models;

public class HouseholdEntitlementValidator
    : AbstractValidator<HouseholdEntitlement>
{
    private static readonly string[] AllowedKishanSchemes =
    {
        "PM Kishan",
        "CM Kishan",
        "Both"
    };

    public HouseholdEntitlementValidator()
    {
        // ---------- KISHAN SCHEME (CASE-INSENSITIVE) ----------
        // RuleFor(x => x.KishanSchemeCoverage)
        //     .NotEmpty()
        //     .Must(v => v.InIgnoreCase(AllowedKishanSchemes))
        //     .WithMessage("Select valid Kishan scheme option");

        // ---------- MGNREGS ----------
        RuleFor(x => x.FullJobCardNumber)
            .NotEmpty()
            .When(x => x.HasMGNREGSJobCard)
            .WithMessage("Job Card Number is required when MGNREGS job card is Yes");

        RuleFor(x => x.FullJobCardNumber)
            .Length(5, 20)
            .When(x => !string.IsNullOrWhiteSpace(x.FullJobCardNumber))
            .WithMessage("Job Card Number length must be between 5 and 20 characters");

        // ---------- BOOLEAN MANDATORY CHECKS ----------
        RuleFor(x => x.HasRuralHousingSchemeHouse).NotNull();
        RuleFor(x => x.HasIndividualHouseholdLatrine).NotNull();
        RuleFor(x => x.HasElectricityConnection).NotNull();
        RuleFor(x => x.IsCoveredUnderAyushmanBharat).NotNull();
        RuleFor(x => x.IsEnrolledUnderShramYogiMaandhan).NotNull();
        RuleFor(x => x.HasJanDhanYojanaAccount).NotNull();
      //  RuleFor(x => x.IsCoveredUnderPMJJBY).NotNull();
      //  RuleFor(x => x.IsCoveredUnderPMSBY).NotNull();

       
    }
}
