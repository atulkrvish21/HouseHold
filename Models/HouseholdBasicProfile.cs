using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HHSurvey.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class HouseholdBasicProfile
{
    public int Id { get; set; }
    public string? UniqueId { get; set; }

    // -------- Location --------
    public string? District { get; set; }
    public string? Block { get; set; }
    public string? GramPanchayat { get; set; }
    public string? RevenueVillage { get; set; }
    public string? Hamlet { get; set; }
    public string? NearestLandmark { get; set; } //New Key

    // -------- Head of Household --------
    public string? HeadOfTheHouseholdNameAsPerAadhar { get; set; }
    public string? HeadOfTheHouseholdGender { get; set; }
    public string? AadharNo { get; set; }
    public string? SocialCategory { get; set; }

    // -------- Bank --------
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? IFSCcodeOrBranch { get; set; }

    // -------- Women Member --------
    public string? WomenMemberName { get; set; }="";
    public int? WomenMemberAge { get; set; }=0;
    public string? WomenMemberMaritalStatus { get; set; }="";
    public string? WomenMemberRelationshipWithHead { get; set; }="";
    public bool? IsWomenCoveredUnderSHG { get; set; } = false;
    public bool? IsWomenCoveredUnderSubhadraYojana { get; set; } = false;

    // -------- Household --------
    public int? TotalFamilyMembers { get; set; }
    public bool HasRationCard { get; set; }
    public string? RationCardNumber { get; set; }
    public string? DrinkingWaterSource { get; set; }
    public bool HasUjjwalaLPGConnection { get; set; }
    public bool? HasLabourCard { get; set; } = false;
    public bool? IsCoveredUnderNSKY { get; set; }=false;
    public string? photoPath {get;set;}
    public string? GeoLocation { get; set; }
    public string? entryBy { get; set; }
    public int sstatus {get;set;} = 0;
    public DateTime EntryDate { get; set; } = DateTime.Now;
    public DateTime? approvedDate {get;set;}
    public string? approvedBy {get;set;} 
    public string? reason {get;set;}
}
