using FluentValidation;
using HHSurvey.Models;

public class HouseholdValidator : AbstractValidator<Household>
{
    public HouseholdValidator(
        IValidator<HouseholdBasicProfile> basicProfileValidator,
        IValidator<HouseholdOccupationAndLand> occupationValidator,
        IValidator<HouseholdMigrationStatus> migrationValidator,
        IValidator<HouseholdEntitlement> entitlementValidator,
        IValidator<HouseholdFamilyMember> familyMemberValidator)
    {
        // ---- BASIC PROFILE ----
        RuleFor(x => x.HouseholdBasicProfile)
            .NotNull().WithMessage("Household basic profile is required")
            .SetValidator(basicProfileValidator);

        // ---- OCCUPATION ----
        RuleFor(x => x.HouseholdOccupationAndLand)
            .NotNull()
            .SetValidator(occupationValidator);

        // ---- MIGRATION ----
        RuleFor(x => x.HouseholdMigrationStatus)
            .SetValidator(migrationValidator!)
            .When(x => x.HouseholdMigrationStatus != null);

        // ---- ENTITLEMENT ----
        RuleFor(x => x.HouseholdEntitlement)
            .SetValidator(entitlementValidator!)
            .When(x => x.HouseholdEntitlement != null);

        // ---- FAMILY MEMBERS (LIST) ----
        RuleForEach(x => x.HouseholdFamilyMember)
            .SetValidator(familyMemberValidator)
            .When(x => x.HouseholdFamilyMember != null);
    }
}
