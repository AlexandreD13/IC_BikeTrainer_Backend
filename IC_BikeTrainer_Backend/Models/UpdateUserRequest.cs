namespace IC_BikeTrainer_Backend.Models;

public class UpdateUserRequest
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
}