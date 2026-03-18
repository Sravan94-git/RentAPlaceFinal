using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs;
using RentAPlace.API.Services;
using System.Security.Claims;

namespace RentAPlace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [Authorize(Roles = "Renter")]
    public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var renterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _bookingService.CreateAsync(dto, renterId);

        if (!result.Success || result.Booking == null)
            return BadRequest(new { message = result.Message });

        return Ok(result.Booking);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Renter")]
    public async Task<ActionResult<IReadOnlyList<BookingResponseDto>>> MyBookings()
    {
        var renterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await _bookingService.GetRenterBookingsAsync(renterId);
        return Ok(list);
    }

    [HttpGet("owner")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<IReadOnlyList<BookingResponseDto>>> OwnerBookings()
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await _bookingService.GetOwnerBookingsAsync(ownerId);
        return Ok(list);
    }

    [HttpPatch("{id:int}/cancel")]
    [Authorize(Roles = "Renter")]
    public async Task<IActionResult> Cancel(int id)
    {
        var renterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _bookingService.CancelByRenterAsync(id, renterId);
        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatusUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _bookingService.UpdateStatusByOwnerAsync(id, ownerId, dto.Status);
        if (!result.Success)
            return NotFound(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
