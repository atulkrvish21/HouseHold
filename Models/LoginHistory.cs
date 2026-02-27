using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HHSurvey.Models;

public class LoginHistory
{
    [Key]
    public int Id { get; set; }


    public string? UserId { get; set; }

    public DateTime LoginTime { get; set; } = DateTime.Now;

    public DateTime? LogoutTime { get; set; }

    [StringLength(50)]
    public string? IpAddress { get; set; }

    [StringLength(255)]
    public string? DeviceInfo { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Success";  // Success or Failed

    public DateTime EntryDate { get; set; } = DateTime.Now;

}
