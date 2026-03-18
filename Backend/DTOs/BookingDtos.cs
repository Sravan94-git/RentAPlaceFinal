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

public class BookingResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string PropertyTitle { get; set; } = "";
    public string PropertyImageUrl { get; set; } = "";
    public string Location { get; set; } = "";
    public int RenterId { get; set; }
    public string RenterName { get; set; } = "";
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int Guests { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class BookingStatusUpdateDto
{
    [Required]
    [RegularExpression("^(Confirmed|Rejected|Cancelled)$", ErrorMessage = "Allowed values: Confirmed, Rejected, Cancelled.")]
    public string Status { get; set; } = "Confirmed";
}
