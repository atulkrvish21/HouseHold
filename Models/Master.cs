using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HHSurvey.Models;
namespace HHSurvey.Models;
public class Relationship 
{
    [Key]
    public int Id { get; set; }
    public string? RelationshipName { get; set; }
     public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class GenderType 
{
    [Key]
    public int Id { get; set; }
    public string? GenderName { get; set; }
     public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class EducationalQualification
{
    [Key]
    public int Id { get; set; }
    public string? QualificationName { get; set; }
     public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class SocialCategory
{
    [Key]
    public int Id { get; set; }
    public string? CategoryName { get; set; }
     public DateTime? entryDate { get; set; } = DateTime.Now;
}   
public class DrinkingWaterSource
{
    [Key]
    public int Id { get; set; }
    public string? SourceName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}


public class RespondentIdentity
{
    [Key]
    public int Id { get; set; }
    public string? IdentityName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}

public class MigrationSector
{
    [Key]
    public int Id { get; set; }
    public string? SectorName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class MigrationPeriod
{
    [Key]
    public int Id { get; set; }
    public int? shortOrder {get;set;}
    public string? PeriodName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}

public class SourcesOfIrrigation
{
    [Key]
    public int Id { get; set; }
    public string? SourceName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class PrimaryOccupation
{
    [Key]
    public int Id { get; set; }
    public string? OccupationName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}


public class ApproximatePrivateLandHolding
{
    [Key]
    public int Id { get; set; }
    public string? HoldingSize { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}

public class InvolvedInLivestockActivity
{
    [Key]
    public int Id { get; set; }
    public string? ActivityType { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}   
public class KishanSchemeCoverage
{
    [Key]
    public int Id { get; set; }
    public string? SchemeName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}   

public class MasterDataResponse
{
    public List<Relationship>? Relationship { get; set; }
    public List<GenderType>? GenderType { get; set; }
    public List<EducationalQualification>? EducationalQualification { get; set; }
    public List<SocialCategory>? SocialCategory { get; set; }
    public List<DrinkingWaterSource>? DrinkingWaterSource { get; set; }
    public List<RespondentIdentity>? RespondentIdentity { get; set; }
    public List<MigrationSector>? MigrationSector { get; set; }
    public List<MigrationPeriod>? MigrationPeriod { get; set; }
    public List<SourcesOfIrrigation>? SourcesOfIrrigation { get; set; }
    public List<PrimaryOccupation>? PrimaryOccupation { get; set; }
    public List<ApproximatePrivateLandHolding>? ApproximatePrivateLandHolding { get; set; }
    public List<InvolvedInLivestockActivity>? InvolvedInLivestockActivity { get; set; }
    public List<KishanSchemeCoverage>? KishanSchemeCoverage { get; set; }
}



