using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BankList
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string BankName { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;

    // Navigation
  
}
