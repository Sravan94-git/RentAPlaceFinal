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
