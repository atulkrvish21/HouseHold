using System.ComponentModel.DataAnnotations;

namespace HHSurvey.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        public string username { get; set; } = string.Empty;
        public string hashedPassword { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
        public string? emailId { get; set; }
        public string? phone { get; set; }
         public string? state { get; set; }
         public string? district { get; set;}
         public string? block { get; set; }
         public string? panchayat { get; set; }
        public int sstatus { get; set; } = 1;
        public int roleId { get; set; } = 1;
        public DateTime entryDate { get; set; } = System.DateTime.Now;
    
    }
}
