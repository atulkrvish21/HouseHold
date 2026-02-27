public class SurveySummaryDto
{
    public string District { get; set; }
    public string Block { get; set; }
    public string Panchayat { get; set; }
    public int TotalVillages { get; set; }
    
    // Metrics
    public int TargetHouseholds { get; set; } // Ideally from a Target Master table
    public int CompletedSurveys { get; set; } // Status = 1 (Approved)
    public int PendingApproval { get; set; }  // Status = 0 (Submitted)
    public int Rejected { get; set; }         // Status = 10
    
    // Computed Property
    public double CompletionPercentage => TargetHouseholds == 0 ? 0 : 
        Math.Round((double)CompletedSurveys / TargetHouseholds * 100, 1);
}