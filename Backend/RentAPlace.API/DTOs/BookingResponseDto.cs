namespace RentAPlace.API.DTOs;

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
