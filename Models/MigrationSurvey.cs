using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MigrationSurvey
{
    [Key]
    public int Id { get; set; }

    // Basic Details
    public string District { get; set; } = string.Empty;  //SL. 1
    public string Block { get; set; } = string.Empty;  //SL.2 
    public string GramPanchayat { get; set; } = string.Empty;  //SL 3
    public string RevenueVillage { get; set; } = string.Empty;  //SL 4
    public int TotalHouseholds { get; set; } // SL5
    public int MalePopulation { get; set; } //SL6
    public int FemalePopulation { get; set; } //SL7
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int TotalPopulation { get; set; } //SL8

    // Basic Infrastructure & Amenities
    public bool? InternalVillageRoads { get; set; }=false;  //REMOVED
    public string? InternalVillageRoadsRequirement { get; set; }=""; //REMOVED
    public bool? InternalDrainsAvailable { get; set; }=false; //REMOVED
    public bool? DrainsProperlyFunctional { get; set; }=false; //REMOVED
    public bool? IsElectrified { get; set; } //SL. 9
    public bool? StreetLightingAvailable { get; set; } //SL. 10
    public string? StreetLightingType { get; set; } //SL. 11
    public string? VillageConnectedToGP { get; set; } //SL. 12
    public double? LengthAllWeatherRoadToGP { get; set; } //SL. 13
    public bool? GPConnectedToPWDOrHighway { get; set; } //REMOVED
    public double? LengthAllWeatherRoadToHighway { get; set; } //REMOVED

    // Migration Info
    public int? MenInMigration { get; set; } //SL. 14
    public int? WomenInMigration { get; set; } //SL. 15
    public int? MinorChildrenInMigration { get; set; } //SL. 16
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int TotalPersonsInMigration { get; set; } //SL. 17

    // Water & Sanitation
    public string? DrinkingWaterSource { get; set; } //SL. 18
    public bool? AllHouseholdsWithToilets { get; set; } //SL. 19

    // Education & Health
    public bool? AnganwadiCentre { get; set; } //SL. 20
    public bool? PrimarySchoolAvailable { get; set; } //SL. 21
    public bool? SecondarySchoolWithin3km { get; set; } //SL. 22
    public bool? SubHealthCentre { get; set; } //SL. 23

    // Community & Social Infrastructure
    public bool? CommunityCentreAvailable { get; set; } //SL. 24
    public bool? CommonShedForWSHG { get; set; } //SL. 25
    public bool? PlaygroundAvailable { get; set; } //SL. 26
    public int? CommunityTanks { get; set; } //SL. 27

    // Livelihood & Service
    public bool? MobileNetworkCoverage { get; set; }=false;  //REMOVED
    public bool? DigitalConnectivity { get; set; } //SL. 28
    public bool? DryingYard { get; set; }=false;  //REMOVED
    public bool? PDSAvailable { get; set; } //SL. 29
    public double? DistanceOfPDS { get; set; } //REMOVED
    public bool? BankingPostOfficeNearby { get; set; } //SL. 30

    // Water Resource & Irrigation
    public bool? WaterFromIrrigationProject { get; set; } //SL. 31
    public bool? RepairOrNewDistributionCanalRequired { get; set; } //REMOVED
    public double? LengthOfDistributionCanal { get; set; } //REMOVED
    public bool? FunctionalLiftIrrigation { get; set; } //REMOVED
    public bool? ScopeOfNewLiftIrrigation { get; set; } //REMOVED
    public bool? FunctionalCheckDams { get; set; } // SL. 32
    public bool? ScopeOfNewCheckDams { get; set; } //REMOVED
    public bool? FunctionalDistributionCanal { get; set; } //REMOVED
    public double? ScopeOfNewDistributionCanal { get; set; } //REMOVED

    // Respondent Details

    public string? RespondentName { get; set; } //SL. 33
    public string? IdentityRole { get; set; } //SL. 34
    public string? SurveyProcess { get; set; } //REMOVED
    public string? RespondentMobile { get; set; } //SL. 35
    public string? MeetingPhotoPath { get; set; }  //REMOVED
    public string? GeoLocation { get; set; } //SL. 36
    public string? EnumeratorName { get; set; } //SL. 37
    public DateTime SurveyDate { get; set; } = DateTime.Now; //SL. 38
}
