using System.ComponentModel.DataAnnotations;

namespace HHSurvey.Models
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        // This field is ignored by the API but included for clarity/safety
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
