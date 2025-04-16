using System.ComponentModel.DataAnnotations;

namespace IC_BikeTrainer_Backend.Models;

public class TrainingSession
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
    
    [Required]
    public double AverageSpeed { get; set; }
    
    [Required]
    public double AverageCadence { get; set; }
    
    [Required]
    public double TotalDistance { get; set; }
    
    [Required]
    public double AverageResistanceLevel { get; set; }
    
    [Required]
    public double AveragePower { get; set; }
    
    [Required]
    public double AveragePowerToWeightRatio { get; set; }
    
    [Required]
    public int TotalEnergy  { get; set; }
    
    [Required]
    public int AverageHeartRate { get; set; }
}