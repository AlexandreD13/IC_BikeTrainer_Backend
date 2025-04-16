using System.ComponentModel.DataAnnotations;

namespace IC_BikeTrainer_Backend.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; } = "";
    
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = "";
    
    [Required]
    [MaxLength(255)]
    public string Firstname { get; set; } = "";
    
    [Required]
    [MaxLength(255)]
    public string Lastname { get; set; } = "";
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = "";
    
    [Required]
    public double? Weight { get; set; }
    
    [Required]
    public double? Height { get; set; }
    
    public AuthRoles Role { get; set; } = AuthRoles.User;

    [Required]
    public DateTime ProfileCreationDate { get; set; } = DateTime.UtcNow;
}