using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class VillageList
{
    [Key]
    public long villageCode { get; set; }

    [ForeignKey("PanchayatList")]
    public long PanchayatCode { get; set; }

    [Required]
    [StringLength(100)]
    public string VillageName { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;


}
