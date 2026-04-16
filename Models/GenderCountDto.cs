public class GenderCountDto
{
   public string DistrictName {get;set;}="";
    public string BlockName {get;set;} = "";
    public string PanchayatName {get;set;} = "";
    public string VillageName {get;set;} = "";
    public int Male { get; set; }
    public int Female { get; set; }
    public int Minor { get; set; }
    public int Total { get; set; }
}