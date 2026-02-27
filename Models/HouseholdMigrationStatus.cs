using System.ComponentModel.DataAnnotations;
namespace HHSurvey.Models;
public class HouseholdMigrationStatus
{
    [Key]
    public int Id { get; set; }
    public string? UniqueId { get; set; }

    // --- Migration ---
    public bool? HasFamilyMemberMigratedLast3Years { get; set; } = false;
    public bool TakenAdvanceForMigrationFromMiddleman { get; set; }
    public int MinorChildrenAccompaniedMigration { get; set; }= 0;
    public bool? WomenMembersMigrated { get; set; } = false;

    // --- Contact / Respondent ---
    public string? FamilyContactMobileNo { get; set; }
    public string? RespondentIdentity { get; set; }
    public string? RespondentPhotoPathOrUrl { get; set; }="";

    public DateTime entryDate { get; set; } = DateTime.Now;
}
