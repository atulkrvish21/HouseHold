using System.ComponentModel.DataAnnotations;
namespace HHSurvey.Models;
public class HouseholdEntitlement
{
    // --- Agriculture Entitlements ---

     [Key]
    public int Id { get; set; }
    public string? UniqueId {get;set;}
    public string? KishanSchemeCoverage { get; set; }

    // --- Housing & Infrastructure ---

    // Has the family provided house under the Rural Housing Scheme?
    public bool HasRuralHousingSchemeHouse { get; set; }

    // Whether the Household provided with Individual Household Latrine in past?
    public bool HasIndividualHouseholdLatrine { get; set; }

    // Whether the household has electricity connection?
    public bool HasElectricityConnection { get; set; }

    // --- Employment & Financial Schemes ---

    // Does your family have a Job Card under MGNREGS?
    public bool HasMGNREGSJobCard { get; set; }
    
    // Mention the Full Job card No ( after Revenue Village code )
    // Note: The specific validation range ("User Entry between...") is context-dependent and omitted here.
    public string? FullJobCardNumber { get; set; } 

    // Does the household have Pradhan Mantri Jan Dhan Yojana bank account?
    public bool HasJanDhanYojanaAccount { get; set; }

    // --- Health & Insurance Schemes ---

    // Whether Covered under Pradhan Mantri Ayushman Jan Arogya Yojana?
    public bool IsCoveredUnderAyushmanBharat { get; set; }

    // Is any household member enrolled under Pradhan Mantri Shram Yogi Maandhan pension scheme?
    public bool IsEnrolledUnderShramYogiMaandhan { get; set; }

    // Whether the family members between 18 to 50 years age covered under PMJJBY?
    public bool? IsCoveredUnderPMJJBY { get; set; } =false;

    // Whether family members between age 18 to 70 years covered under PMSBY?
    public bool? IsCoveredUnderPMSBY { get; set; } = false;
      public bool? AtalPension { get; set; } = false;
      public bool? OldagePension { get; set; } = false;
      public bool? WidowPension { get; set; } = false;
      public bool? DisabilityPension { get; set; } = false; 
     public DateTime? entryDate { get; set; } = DateTime.Now;
}