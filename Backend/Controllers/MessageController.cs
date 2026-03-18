using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.Models;
using System.Security.Claims;

namespace RentAPlace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly AppDbContext _context;

    public MessageController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyMessages()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
            return Unauthorized();

        var messages = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Include(m => m.Property)
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new
            {
                id = m.Id,
                propertyId = m.PropertyId,
                propertyTitle = m.Property!.Title,
                senderId = m.SenderId,
                senderName = m.Sender!.FullName,
                receiverId = m.ReceiverId,
                receiverName = m.Receiver!.FullName,
                content = m.Content,
                createdAt = m.CreatedAt,
                isRead = m.IsRead
            })
            .ToListAsync();

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int senderId))
            return Unauthorized();

        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = dto.ReceiverId,
            PropertyId = dto.PropertyId,
            Content = dto.Content.Trim(),
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(new { message.Id, message.Content, message.CreatedAt });
    }
}

public class SendMessageDto
{
    public int PropertyId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; } = "";
}
