using System.ComponentModel.DataAnnotations;
namespace HHSurvey.Models;
public class HouseholdOccupationAndLand
{
    [Key]
    public int Id { get; set; }

    public string? UniqueId { get; set; }

    // -------- Occupation --------
    public string? PrimaryOccupationOfTheFamily { get; set; }
    public string? OtherPrimaryOccupationDetails { get; set; }

    public bool? IsFamilyInvolvedInWeavingOrHandloom { get; set; } = false;
    public bool? IsFamilyCoveredUnderPOHI_LoomsScheme { get; set; } = false;

    // -------- FRA --------
    public string? FRAClaimantStatus { get; set; }="";
    public decimal? FRA_LandAmountInAcres { get; set; }=0;

    // -------- Land --------
    public bool OwnsHomesteadPattaLand { get; set; }
    public string? ApproximatePrivateLandHolding { get; set; }

    // -------- Irrigation --------
    public bool IsIrrigationFacilityAvailable { get; set; }
    public string? SourcesOfIrrigation { get; set; }

    // -------- Livestock --------
    public string? InvolvedInLivestockActivity { get; set; }="";

    public DateTime entryDate { get; set; } = DateTime.Now;
}
