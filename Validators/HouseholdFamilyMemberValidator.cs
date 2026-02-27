using FluentValidation;
using HHSurvey.Models;

public class HouseholdFamilyMemberValidator
    : AbstractValidator<HouseholdFamilyMember>
{
    private static readonly string[] AllowedGenders =
    {
        "Male", "Female", "Other"
    };

    private static readonly string[] AllowedEducation =
    {
        "Illiterate", "Primary", "Middle", "Matric",
        "Intermediate", "Graduate", "PostGraduate", "Other"
    };

    private static readonly string[] AllowedSectors =
    {
        "Brick Kiln",
        "Construction Labour",
        "Agri Labour",
        "Mason",
        "Domestic Support",
        "Manufacturing",
        "Service Sector (Hotel, Hospital, Security)",
        "Other"
    };

    private static readonly string[] AllowedPeriods =
    {
        "1-3 months",
        "4-6 months",
        "7-12 months"
    };

    private static readonly string[] AllowedSkillInterest =
    {
        "DDUGKY", "RSETI", "Other", "None"
    };

    public HouseholdFamilyMemberValidator()
    {
       
        // ---------- BASIC DETAILS ----------
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        // RuleFor(x => x.Age)
        //     .InclusiveBetween(0, 100);

        RuleFor(x => x.Gender)
            .NotEmpty()
            .Must(v => v.InIgnoreCase(AllowedGenders))
            .WithMessage("Invalid gender");

        RuleFor(x => x.EducationalQualification)
            .NotEmpty()
            .WithMessage("Invalid educational qualification");

        // ---------- MIGRATION CONDITIONAL LOGIC ----------
        // RuleFor(x => x.DestinationState)
        //     .NotEmpty()
        //     .When(x => x.MigratedInLast3Years)
        //     .WithMessage("Destination State is required if migrated");

        RuleFor(x => x.SectorOfEngagementDuringMigration)
            .NotEmpty()
            .When(x => x.MigratedInLast3Years)
            .Must(v => v.InIgnoreCase(AllowedSectors))
            .WithMessage("Invalid sector of engagement");

        // RuleFor(x => x.PeriodOfMigration)
        //     .NotEmpty()
        //     .When(x => x.MigratedInLast3Years)
        //     .Must(v => v.InIgnoreCase(AllowedPeriods))
        //     .WithMessage("Invalid period of migration");

        RuleFor(x => x.MonthlyRemittanceDuringMigration)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MigratedInLast3Years)
            .WithMessage("Monthly remittance must be zero or more");

        // ---------- NON-MIGRANT CLEANUP ----------
        RuleFor(x => x.MonthlyRemittanceDuringMigration)
            .Equal(0)
            .When(x => !x.MigratedInLast3Years)
            .WithMessage("Remittance must be 0 if not migrated");

        // ---------- SKILL DEVELOPMENT ----------
        RuleFor(x => x.InterestInSkillDevelopment)
            .NotEmpty()
            .Must(v => v.InIgnoreCase(AllowedSkillInterest))
            .WithMessage("Invalid skill development option");

        
    }
}
