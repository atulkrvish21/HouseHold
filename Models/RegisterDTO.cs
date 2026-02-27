using System.ComponentModel.DataAnnotations;

namespace HHSurvey.Models
{
    public class RegisterDTO
    {
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        [Required]
        public string fullName { get; set; } = string.Empty;
        public string? emailId { get; set; }
        public string? phone { get; set; }
         public string? state { get; set; }
         public string? district { get; set;}
         public string? block { get; set; }
         public string? panchayat { get; set; }

    }
}
