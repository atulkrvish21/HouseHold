using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PanchayatList
{
    [Key]
    public long PanchayatCode { get; set; }

    [ForeignKey("BlockList")]
    public int blockCode { get; set; }

    [Required]
    [StringLength(100)]
    public string PanchayatName { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;


}
