using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services;

public interface IPropertyService
{
    Task<PagedResultDto<PropertyResponseDto>> SearchAsync(PropertyQueryDto query);
    Task<PropertyResponseDto?> GetByIdAsync(int id);
    Task<PropertyResponseDto> CreateAsync(PropertyCreateDto dto, int ownerId);
    Task<PropertyResponseDto?> UpdateAsync(int id, PropertyCreateDto dto, int ownerId);
    Task<bool> DeleteAsync(int id, int ownerId);
    Task<IReadOnlyList<PropertyResponseDto>> GetOwnerPropertiesAsync(int ownerId);
}
