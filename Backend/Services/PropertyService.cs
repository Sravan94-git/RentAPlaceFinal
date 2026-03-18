using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services;

public class PropertyService : IPropertyService
{
    private readonly AppDbContext _context;

    public PropertyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<PropertyResponseDto>> SearchAsync(PropertyQueryDto query)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 9 : Math.Min(query.PageSize, 50);

        var q = _context.Properties.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            var location = query.Location.Trim().ToLower();
            q = q.Where(x => x.Location.ToLower().Contains(location));
        }

        if (!string.IsNullOrWhiteSpace(query.PropertyType))
        {
            var type = query.PropertyType.Trim().ToLower();
            q = q.Where(x => x.PropertyType.ToLower() == type);
        }

        if (query.MinPrice.HasValue)
            q = q.Where(x => x.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            q = q.Where(x => x.Price <= query.MaxPrice.Value);

        if (query.HasPool.HasValue)
            q = q.Where(x => x.HasPool == query.HasPool.Value);

        if (query.IsBeachFacing.HasValue)
            q = q.Where(x => x.IsBeachFacing == query.IsBeachFacing.Value);

        if (query.HasGarden.HasValue)
            q = q.Where(x => x.HasGarden == query.HasGarden.Value);

        if (query.Guests.HasValue)
            q = q.Where(x => x.MaxGuests >= query.Guests.Value);

        var totalCount = await q.CountAsync();

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            if (query.SortBy == "price_asc")
                q = q.OrderBy(x => x.Price);
            else if (query.SortBy == "price_desc")
                q = q.OrderByDescending(x => x.Price);
            else // Default to recommended
                q = q.OrderByDescending(x => x.Rating).ThenBy(x => x.Price);
        }
        else
        {
            q = q.OrderByDescending(x => x.Rating).ThenBy(x => x.Price);
        }

        var items = await q
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.ToDto())
            .ToListAsync();

        return new PagedResultDto<PropertyResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PropertyResponseDto?> GetByIdAsync(int id)
    {
        var property = await _context.Properties.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return property?.ToDto();
    }

    public async Task<PropertyResponseDto> CreateAsync(PropertyCreateDto dto, int ownerId)
    {
        var entity = new Property
        {
            Title = dto.Title.Trim(),
            Location = dto.Location.Trim(),
            PropertyType = dto.PropertyType.Trim(),
            Price = dto.Price,
            MaxGuests = dto.MaxGuests,
            Description = dto.Description.Trim(),
            ImageUrl = dto.ImageUrl.Trim(),
            HasPool = dto.HasPool,
            IsBeachFacing = dto.IsBeachFacing,
            HasGarden = dto.HasGarden,
            OwnerId = ownerId
        };

        _context.Properties.Add(entity);
        await _context.SaveChangesAsync();
        return entity.ToDto();
    }

    public async Task<PropertyResponseDto?> UpdateAsync(int id, PropertyCreateDto dto, int ownerId)
    {
        var entity = await _context.Properties.FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == ownerId);
        if (entity == null)
            return null;

        entity.Title = dto.Title.Trim();
        entity.Location = dto.Location.Trim();
        entity.PropertyType = dto.PropertyType.Trim();
        entity.Price = dto.Price;
        entity.MaxGuests = dto.MaxGuests;
        entity.Description = dto.Description.Trim();
        entity.ImageUrl = dto.ImageUrl.Trim();
        entity.HasPool = dto.HasPool;
        entity.IsBeachFacing = dto.IsBeachFacing;
        entity.HasGarden = dto.HasGarden;

        await _context.SaveChangesAsync();
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, int ownerId)
    {
        var entity = await _context.Properties.FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == ownerId);
        if (entity == null)
            return false;

        _context.Properties.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IReadOnlyList<PropertyResponseDto>> GetOwnerPropertiesAsync(int ownerId)
    {
        return await _context.Properties
            .AsNoTracking()
            .Where(x => x.OwnerId == ownerId)
            .OrderByDescending(x => x.Id)
            .Select(x => x.ToDto())
            .ToListAsync();
    }
}
