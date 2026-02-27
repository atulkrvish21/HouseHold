using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HHSurvey.Models;
public class Household
{
    
    public HouseholdBasicProfile? HouseholdBasicProfile { get; set;}
    public HouseholdEntitlement? HouseholdEntitlement { get; set;}
    public HouseholdMigrationStatus? HouseholdMigrationStatus{ get; set;}
    public HouseholdOccupationAndLand? HouseholdOccupationAndLand{ get; set;}
    public List<HouseholdFamilyMember>? HouseholdFamilyMember{ get;set;}
 
}

