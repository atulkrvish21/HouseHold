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

public class DrinkingWaterSourceType
{
    [Key]
    public int Id { get; set; }
    public string? SourceTypeName { get; set; }
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
    public string? PeriodName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}

public class SkillDevelopmentInterest
{
    [Key]
    public int Id { get; set; }
    public string? InterestName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class MigrationDestinationState
{
    [Key]
    public int Id { get; set; }
    public string? StateName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}
public class MigrationAdvanceSource
{
    [Key]
    public int Id { get; set; }
    public string? SourceName { get; set; }
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

public class WeavingOrHandloomInvolvement
{
    [Key]
    public int Id { get; set; }
    public string? InvolvementType { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}   

public class FRAClaimantStatus
{
    [Key]
    public int Id { get; set; }
    public string? StatusName { get; set; }
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

public class HouseholdType
{
    [Key]
    public int Id { get; set; }
    public string? TypeName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}   

public class LandOwnershipStatus
{
    [Key]
    public int Id { get; set; }
    public string? StatusName { get; set; }
    public DateTime? entryDate { get; set; } = DateTime.Now;
}


