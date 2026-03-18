namespace RentAPlace.API.DTOs;

public class PropertyQueryDto
{
    public string? Location { get; set; }
    public string? PropertyType { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? HasPool { get; set; }
    public bool? IsBeachFacing { get; set; }
    public bool? HasGarden { get; set; }
    public int? Guests { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}
