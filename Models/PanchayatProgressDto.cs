public class PanchayatProgressDto
{
    public string District { get; set; }
    public string Block { get; set; }
    public string Panchayat { get; set; }
    public long PanchayatCode { get; set; }
    public int TotalVillages { get; set; }

    // CHANGED: Instead of Target, we show Total Submissions
    public int TotalSubmitted { get; set; } 
    public int Completed { get; set; }
    public int Pending { get; set; }
    public int Rejected { get; set; }

    // Optional: Calculate Approval Rate (Completed vs Total Submitted)
    public double ApprovalRate => TotalSubmitted == 0 ? 0 : 
        Math.Round((double)Completed / TotalSubmitted * 100, 1);
}