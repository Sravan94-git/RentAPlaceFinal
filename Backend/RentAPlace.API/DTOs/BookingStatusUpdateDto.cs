using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs;

public class BookingStatusUpdateDto
{
    [Required]
    [RegularExpression("^(Confirmed|Rejected|Cancelled)$", ErrorMessage = "Allowed values: Confirmed, Rejected, Cancelled.")]
    public string Status { get; set; } = "Confirmed";
}
