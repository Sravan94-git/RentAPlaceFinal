namespace RentAPlace.API.DTOs;

public class PropertyResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Location { get; set; } = "";
    public string PropertyType { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public bool HasPool { get; set; }
    public bool IsBeachFacing { get; set; }
    public bool HasGarden { get; set; }
    public int MaxGuests { get; set; }
    public decimal Rating { get; set; }
    public int OwnerId { get; set; }
}
