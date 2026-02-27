using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HHSurvey.Models;
public class DashboardDTO
{
    
    public int? TotalHousehold { get; set;}
    public int? Approved { get; set;}
    public int? Pending{ get; set;}
    public int TotalHouseholdFTM { get; set;}
    public int? ApprovedFTM { get; set;}
    public int? PendingFTM { get; set;}
    
}

