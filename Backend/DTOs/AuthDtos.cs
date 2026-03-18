using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = "";
    public string Role { get; set; } = "";
    public string Name { get; set; } = "";
    public int UserId { get; set; }
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}

public class RegisterDto
{
    [MaxLength(100)]
    public string? FullName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = "";

    [Required]
    [RegularExpression("^(Owner|Renter)$", ErrorMessage = "Role must be Owner or Renter.")]
    public string Role { get; set; } = "Renter";
}
