using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MigrationSurvey
{
    [Key]
    public int Id { get; set; }

    // Basic Details
    public string District { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string GramPanchayat { get; set; } = string.Empty;
    public string RevenueVillage { get; set; } = string.Empty;
    public int TotalHouseholds { get; set; }
    public int MalePopulation { get; set; }
    public int FemalePopulation { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int TotalPopulation { get; set; }

    // Basic Infrastructure & Amenities
    public bool? InternalVillageRoads { get; set; }=false;
    public string? InternalVillageRoadsRequirement { get; set; }="";
    public bool? InternalDrainsAvailable { get; set; }=false;
    public bool? DrainsProperlyFunctional { get; set; }=false;
    public bool? IsElectrified { get; set; }
    public bool? StreetLightingAvailable { get; set; }
    public string? StreetLightingType { get; set; }
    public bool? VillageConnectedToGP { get; set; }
    public double? LengthAllWeatherRoadToGP { get; set; }
    public bool? GPConnectedToPWDOrHighway { get; set; }
    public double? LengthAllWeatherRoadToHighway { get; set; }

    // Migration Info
    public int? MenInMigration { get; set; }
    public int? WomenInMigration { get; set; }
    public int? MinorChildrenInMigration { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int TotalPersonsInMigration { get; set; }

    // Water & Sanitation
    public string? DrinkingWaterSource { get; set; }
    public bool? AllHouseholdsWithToilets { get; set; }

    // Education & Health
    public bool? AnganwadiCentre { get; set; }
    public bool? PrimarySchoolAvailable { get; set; }
    public bool? SecondarySchoolWithin3km { get; set; }
    public bool? SubHealthCentre { get; set; }

    // Community & Social Infrastructure
    public bool? CommunityCentreAvailable { get; set; }
    public bool? CommonShedForWSHG { get; set; }
    public bool? PlaygroundAvailable { get; set; }
    public int? CommunityTanks { get; set; }

    // Livelihood & Service
    public bool? MobileNetworkCoverage { get; set; }=false;
    public bool? DigitalConnectivity { get; set; }
    public bool? DryingYard { get; set; }=false;
    public bool? PDSAvailable { get; set; }
    public double? DistanceOfPDS { get; set; }
    public bool? BankingPostOfficeNearby { get; set; }

    // Water Resource & Irrigation
    public bool? WaterFromIrrigationProject { get; set; }
    public bool? RepairOrNewDistributionCanalRequired { get; set; }
    public double? LengthOfDistributionCanal { get; set; }
    public bool? FunctionalLiftIrrigation { get; set; }
    public bool? ScopeOfNewLiftIrrigation { get; set; }
    public bool? FunctionalCheckDams { get; set; }
    public bool? ScopeOfNewCheckDams { get; set; }
    public bool? FunctionalDistributionCanal { get; set; }
    public double? ScopeOfNewDistributionCanal { get; set; }

    // Respondent Details

    public string? RespondentName { get; set; }
    public string? IdentityRole { get; set; }
    public string? SurveyProcess { get; set; }
    public string? RespondentMobile { get; set; }
    public string? MeetingPhotoPath { get; set; }
    public string? GeoLocation { get; set; }
    public string? EnumeratorName { get; set; }
    public DateTime SurveyDate { get; set; } = DateTime.Now;
}
