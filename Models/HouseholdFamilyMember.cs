using System.ComponentModel.DataAnnotations;
namespace HHSurvey.Models;
public class HouseholdFamilyMember
{
    [Key]
    public int Id { get; set; }
    public string? UniqueId {get;set;}
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; } // Reusing GenderType enum from the first model
    public string? EducationalQualification { get; set; }

    // --- Migration Details ---
    public bool MigratedInLast3Years { get; set; }
    
    // Only relevant if MigratedInLast3Years is true
    public string? DestinationState { get; set; }
    
    // Nature/ Sector of engagement
    public string? SectorOfEngagementDuringMigration { get; set; }
    
    // Period Of Migration
    public string? PeriodOfMigration { get; set; }
    
    // What was the monthly remittance during migration (In Rs)?
    public int? MonthlyRemittanceDuringMigration { get; set; }
    
    // Whether interested for skill development
    public bool? InterestInSkillDevelopment { get; set; }=false;

    public string? RelationshipWithHeadOfHousehold { get; set; } = "";
    public bool? memberHasLabourCard { get; set; } = false;
    public bool? memberCoveredUnderNSKY {get;set;} = false;
    public DateTime? entryDate { get; set; } = DateTime.Now;
}