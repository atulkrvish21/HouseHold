using FluentValidation;
using HHSurvey.Models;

public class HouseholdMigrationStatusValidator
    : AbstractValidator<HouseholdMigrationStatus>
{
    private static readonly string[] AllowedRespondents =
    {
        "Migrant Person himself",
        "Other Adult family member",
        "Village Head / Ward Member",
        "Neighbor",
        "Head of the Household"
    };

    public HouseholdMigrationStatusValidator()
    {
        // ---------- MIGRATION LOGIC ----------
       
        // ---------- MOBILE NUMBER ----------
        RuleFor(x => x.FamilyContactMobileNo)
            .NotEmpty()
            .Matches(@"^[6-9]\d{9}$")
            .WithMessage("Enter a valid 10-digit mobile number");

        // ---------- RESPONDENT (CASE-INSENSITIVE) ----------
        RuleFor(x => x.RespondentIdentity)
            .NotEmpty()
            .Must(v => v.InIgnoreCase(AllowedRespondents))
            .WithMessage("Invalid respondent identity");

        // ---------- PHOTO ----------
        // RuleFor(x => x.RespondentPhotoPathOrUrl)
        //     .Must(BeValidPhoto)
        //     .When(x => !string.IsNullOrWhiteSpace(x.RespondentPhotoPathOrUrl))
        //     .WithMessage("Photo must be JPG/JPEG and ≤ 1 MB");
    }

    private bool BeValidPhoto(string path)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg" };

        return allowedExtensions.Any(ext =>
            path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }
}
