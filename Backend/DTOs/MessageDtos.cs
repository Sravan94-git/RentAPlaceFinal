using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs;

public class MessageCreateDto
{
    [Required]
    public int PropertyId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ReceiverId { get; set; }

    [Required]
    [MaxLength(1200)]
    public string Content { get; set; } = "";
}

public class MessageResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string PropertyTitle { get; set; } = "";
    public int SenderId { get; set; }
    public string SenderName { get; set; } = "";
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
