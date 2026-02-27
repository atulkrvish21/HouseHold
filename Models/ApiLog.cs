using System;
using System.ComponentModel.DataAnnotations;

public class ApiLog
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public string? Body { get; set; }
public string? ResponseBody { get; set; }
    public int? ResponseStatus { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceInfo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
