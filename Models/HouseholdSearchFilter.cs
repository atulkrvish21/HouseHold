public class HouseholdSearchFilter
{
    // Date filter (mandatory)
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    // Location filters
    public string? District { get; set; }
    public string? Block { get; set; }
    public string? GramPanchayat { get; set; }
    public string? RevenueVillage { get; set; }

    // Social filters
    public string? SocialCategory { get; set; }

    // Flags
    public bool? HasRationCard { get; set; }
    public bool? IsCoveredUnderSHG { get; set; }
    public int? status { get; set; }
    // Entry filters
    public string? EntryBy { get; set; }

     // Pagination
    public int PageNumber { get; set; } = 1;   // default
    public int PageSize { get; set; } = 10;    // default
    
}

public class PagedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public List<T> Data { get; set; } = new();
}