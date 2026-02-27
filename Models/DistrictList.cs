using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DistrictList
{
    [Key]
    public int DistrictCode { get; set; }

    [ForeignKey("StateList")]
    public int stateCode { get; set; }

    [Required]
    [StringLength(100)]
    public string DistrictName { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;

 
}
