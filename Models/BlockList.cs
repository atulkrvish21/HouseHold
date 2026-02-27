using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BlockList
{
    [Key]
    public int BlockCode { get; set; }

    [ForeignKey("DistrictList")]
    public int DistrictCode { get; set; }  

    [Required]
    [StringLength(100)]
    public string BlockName { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;

   
}
