public class EntitlementGapDto
{
    public string District { get; set; }
    public string Block { get; set; }
    public string Panchayat { get; set; }
    
    public int TotalHouseholds { get; set; }
    
    // The "Gap" Counts (Households missing schemes)
    public int NoRationCard { get; set; }
    public int NoHousing { get; set; }
    public int NoToilet { get; set; }
    public int WomenNotInSHG { get; set; }
}