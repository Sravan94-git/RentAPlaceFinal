using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using RentAPlace.API.Models;
using RentAPlace.API.Services;

namespace RentAPlace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext context, PasswordService passwordService, JwtService jwtService)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var normalizedEmail = dto.Email.Trim().ToLower();
        var role = dto.Role.Trim().Equals("Owner", StringComparison.OrdinalIgnoreCase) ? "Owner" : "Renter";

        var exists = await _context.Users.AnyAsync(x => x.Email == normalizedEmail);
        if (exists)
            return Conflict(new { message = "Email already exists." });

        var finalName = !string.IsNullOrWhiteSpace(dto.FullName) 
            ? dto.FullName.Trim() 
            : $"{dto.FirstName} {dto.LastName}".Trim();

        if (string.IsNullOrWhiteSpace(finalName))
            return BadRequest(new { message = "Full Name or First/Last name must be provided." });

        var user = new User
        {
            FullName = finalName,
            Email = normalizedEmail,
            PasswordHash = _passwordService.HashPassword(dto.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var normalizedEmail = dto.Email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail);

        if (user == null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
            return BadRequest(new { message = "Invalid credentials." });

        var token = _jwtService.GenerateToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Role = user.Role,
            Name = user.FullName,
            UserId = user.Id
        });
    }
}
