using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.Messaging.API.Data;
using RentAPlace.Messaging.API.DTOs;
using RentAPlace.Messaging.API.Models;
using System.Security.Claims;

namespace RentAPlace.Messaging.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly MessagingDbContext _context;

    public MessageController(MessagingDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponseDto>> Send([FromBody] MessageCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (senderId == dto.ReceiverId)
            return BadRequest(new { message = "Cannot send message to yourself." });

        var property = await _context.Properties.FirstOrDefaultAsync(x => x.Id == dto.PropertyId);
        if (property == null)
            return NotFound(new { message = "Property not found." });

        var receiver = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.ReceiverId);
        if (receiver == null)
            return NotFound(new { message = "Receiver not found." });

        var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId);
        if (sender == null)
            return NotFound(new { message = "Sender not found." });

        var message = new Message
        {
            PropertyId = dto.PropertyId,
            SenderId = senderId,
            ReceiverId = dto.ReceiverId,
            Content = dto.Content.Trim()
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(new MessageResponseDto
        {
            Id = message.Id,
            PropertyId = message.PropertyId,
            PropertyTitle = property.Title,
            SenderId = sender.Id,
            SenderName = sender.FullName,
            ReceiverId = receiver.Id,
            ReceiverName = receiver.FullName,
            Content = message.Content,
            IsRead = message.IsRead,
            CreatedAt = message.CreatedAt
        });
    }

    [HttpGet("my")]
    public async Task<ActionResult<IReadOnlyList<MessageResponseDto>>> MyMessages()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var list = await _context.Messages
            .AsNoTracking()
            .Include(x => x.Property)
            .Include(x => x.Sender)
            .Include(x => x.Receiver)
            .Where(x => x.SenderId == userId || x.ReceiverId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MessageResponseDto
            {
                Id = x.Id,
                PropertyId = x.PropertyId,
                PropertyTitle = x.Property!.Title,
                SenderId = x.SenderId,
                SenderName = x.Sender!.FullName,
                ReceiverId = x.ReceiverId,
                ReceiverName = x.Receiver!.FullName,
                Content = x.Content,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return Ok(list);
    }
}
