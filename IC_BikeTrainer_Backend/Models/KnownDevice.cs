using System.ComponentModel.DataAnnotations;

namespace IC_BikeTrainer_Backend.Models;

public class KnownDevice
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string DeviceName { get; set; } = "";
    
    [Required]
    [MaxLength(255)]
    public string MacAddress { get; set; } = "";
}