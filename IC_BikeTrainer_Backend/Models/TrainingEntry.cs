using System.ComponentModel.DataAnnotations;

namespace IC_BikeTrainer_Backend.Models;

public class TrainingEntry
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int TrainingSessionId { get; set; }
    
    [Required]
    public DateTime EntryTimestamp { get; set; }
    
    [Required]
    public double InstantaneousSpeed { get; set; }
    
    [Required]
    public int InstantaneousCadence { get; set; }
    
    [Required]
    public double CumulativeDistance { get; set; }
    
    [Required]
    public int ResistanceLevel { get; set; }
    
    [Required]
    public int InstantaneousPower { get; set; }
    
    [Required]
    public double PowerToWeightRatio { get; set; }
    
    [Required]
    public int CumulativeEnergy { get; set; }
    
    [Required]
    public int HeartRate { get; set; }
}