namespace HHSurvey.Models
{
    public class UserLoginDTO
    {

        public string username { get; set; } = string.Empty;
        public string hashedPassword { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
        public string? emailId { get; set; }
        public string? phone { get; set; }
        public string? officeLocation { get; set; }
        public int sstatus { get; set; }
        public string RoleDesc { get; set; } = string.Empty;

        


    }
}
