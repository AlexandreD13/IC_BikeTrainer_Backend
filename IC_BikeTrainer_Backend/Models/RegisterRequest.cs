namespace IC_BikeTrainer_Backend.Models;

public class RegisterRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Firstname { get; set; } = "";
    public string Lastname { get; set; } = "";
    public string Email { get; set; } = "";
    public double? Weight { get; set; }
    public double? Height { get; set; }
}