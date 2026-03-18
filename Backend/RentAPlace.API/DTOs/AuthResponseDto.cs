namespace RentAPlace.API.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = "";
    public string Role { get; set; } = "";
    public string Name { get; set; } = "";
    public int UserId { get; set; }
}
