namespace RentAPlace.Messaging.API.DTOs;

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
