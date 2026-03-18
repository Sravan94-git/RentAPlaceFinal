using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs;
using RentAPlace.API.Services;
using System.Security.Claims;

namespace RentAPlace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IWebHostEnvironment _environment;

    public PropertyController(IPropertyService propertyService, IWebHostEnvironment environment)
    {
        _propertyService = propertyService;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<PropertyResponseDto>>> GetAll([FromQuery] PropertyQueryDto query)
    {
        var result = await _propertyService.SearchAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropertyResponseDto>> GetById(int id)
    {
        var item = await _propertyService.GetByIdAsync(id);
        if (item == null)
            return NotFound(new { message = "Property not found." });

        return Ok(item);
    }

    [Authorize(Roles = "Owner")]
    [HttpPost]
    public async Task<ActionResult<PropertyResponseDto>> Add([FromBody] PropertyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var property = await _propertyService.CreateAsync(dto, ownerId);
        return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PropertyResponseDto>> Update(int id, [FromBody] PropertyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await _propertyService.UpdateAsync(id, dto, ownerId);
        if (updated == null)
            return NotFound(new { message = "Property not found." });

        return Ok(updated);
    }

    [Authorize(Roles = "Owner")]
    [HttpGet("my")]
    public async Task<ActionResult<IReadOnlyList<PropertyResponseDto>>> MyProperties()
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var list = await _propertyService.GetOwnerPropertiesAsync(ownerId);
        return Ok(list);
    }

    [Authorize(Roles = "Owner")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var deleted = await _propertyService.DeleteAsync(id, ownerId);
        if (!deleted)
            return NotFound(new { message = "Property not found." });

        return Ok(new { message = "Property deleted successfully." });
    }

    [Authorize(Roles = "Owner")]
    [HttpPost("upload-image")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Please provide an image file." });

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        if (!allowed.Contains(extension))
            return BadRequest(new { message = "Only jpg, jpeg, png, and webp are allowed." });

        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(webRootPath))
        {
            webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadsFolder = Path.Combine(webRootPath, "uploads");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream);

        var relativePath = $"/uploads/{fileName}";
        return Ok(new { imageUrl = relativePath });
    }
}
