using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs;

public class PropertyCreateDto
{
    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(120)]
    public string Location { get; set; } = "";

    [Required]
    [MaxLength(40)]
    public string PropertyType { get; set; } = "Apartment";

    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Range(1, 30)]
    public int MaxGuests { get; set; } = 2;

    [MaxLength(2000)]
    public string Description { get; set; } = "";

    public string ImageUrl { get; set; } = "";
    public bool HasPool { get; set; }
    public bool IsBeachFacing { get; set; }
    public bool HasGarden { get; set; }
}
