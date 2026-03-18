using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs;

public class BookingCreateDto
{
    [Required]
    public int PropertyId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 30)]
    public int Guests { get; set; }
}
