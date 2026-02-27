using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class StateList
{
    [Key]
    public int StateCode { get; set; }

    [Required]
    [StringLength(100)]
    public string StateName { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;

    // Navigation
  
}
