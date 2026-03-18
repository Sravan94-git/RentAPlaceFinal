using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<BookingService> _logger;

    public BookingService(AppDbContext context, IEmailService emailService, ILogger<BookingService> logger)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<(bool Success, string Message, BookingResponseDto? Booking)> CreateAsync(BookingCreateDto dto, int renterId)
    {
        if (dto.CheckOutDate.Date <= dto.CheckInDate.Date)
            return (false, "Check-out date must be after check-in date.", null);

        var property = await _context.Properties.FirstOrDefaultAsync(x => x.Id == dto.PropertyId);
        if (property == null)
            return (false, "Property not found.", null);

        if (property.OwnerId == renterId)
            return (false, "Owners cannot book their own properties.", null);

        if (dto.Guests > property.MaxGuests)
            return (false, $"This property supports up to {property.MaxGuests} guests.", null);

        var hasOverlap = await _context.Bookings.AnyAsync(x =>
            x.PropertyId == dto.PropertyId &&
            x.Status != "Cancelled" &&
            x.Status != "Rejected" &&
            dto.CheckInDate.Date < x.CheckOutDate.Date &&
            dto.CheckOutDate.Date > x.CheckInDate.Date);

        if (hasOverlap)
            return (false, "The selected dates are not available.", null);

        var nights = (dto.CheckOutDate.Date - dto.CheckInDate.Date).Days;
        var totalPrice = nights * property.Price;

        var entity = new Booking
        {
            PropertyId = dto.PropertyId,
            RenterId = renterId,
            CheckInDate = dto.CheckInDate.Date,
            CheckOutDate = dto.CheckOutDate.Date,
            Guests = dto.Guests,
            TotalPrice = totalPrice,
            Status = "Pending"
        };

        _context.Bookings.Add(entity);
        await _context.SaveChangesAsync();

        var booking = await _context.Bookings
            .Include(x => x.Property)
            .ThenInclude(x => x!.Owner)
            .Include(x => x.Renter)
            .FirstAsync(x => x.Id == entity.Id);

        await TrySendBookingCreatedEmails(booking);

        return (true, "Booking created successfully.", booking.ToDto());
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetRenterBookingsAsync(int renterId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(x => x.Property)
            .Include(x => x.Renter)
            .Where(x => x.RenterId == renterId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    public async Task<IReadOnlyList<BookingResponseDto>> GetOwnerBookingsAsync(int ownerId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(x => x.Property)
            .Include(x => x.Renter)
            .Where(x => x.Property != null && x.Property.OwnerId == ownerId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    public async Task<(bool Success, string Message)> CancelByRenterAsync(int bookingId, int renterId)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId && x.RenterId == renterId);
        if (booking == null)
            return (false, "Booking not found.");

        if (booking.Status == "Cancelled")
            return (false, "Booking is already cancelled.");

        booking.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return (true, "Booking cancelled.");
    }

    public async Task<(bool Success, string Message)> UpdateStatusByOwnerAsync(int bookingId, int ownerId, string status)
    {
        var booking = await _context.Bookings
            .Include(x => x.Property)
            .FirstOrDefaultAsync(x => x.Id == bookingId && x.Property != null && x.Property.OwnerId == ownerId);

        if (booking == null)
            return (false, "Booking not found.");

        if (booking.Status == "Cancelled" && status != "Cancelled")
            return (false, "Cancelled booking cannot be changed.");

        booking.Status = status;
        await _context.SaveChangesAsync();

        await TrySendBookingStatusEmail(booking.Id);
        return (true, "Booking status updated.");
    }

    private async Task TrySendBookingCreatedEmails(Booking booking)
    {
        try
        {
            var ownerEmail = booking.Property?.Owner?.Email;
            var renterEmail = booking.Renter?.Email;

            if (!string.IsNullOrWhiteSpace(ownerEmail))
            {
                await _emailService.SendAsync(
                    ownerEmail,
                    "New booking request on RentAPlace",
                    $"<p>You received a new booking request for <strong>{booking.Property?.Title}</strong>.</p>" +
                    $"<p>Dates: {booking.CheckInDate:yyyy-MM-dd} to {booking.CheckOutDate:yyyy-MM-dd}</p>" +
                    $"<p>Guests: {booking.Guests}</p>");
            }

            if (!string.IsNullOrWhiteSpace(renterEmail))
            {
                await _emailService.SendAsync(
                    renterEmail,
                    "Booking request submitted",
                    $"<p>Your booking request for <strong>{booking.Property?.Title}</strong> was submitted.</p>" +
                    $"<p>Status: {booking.Status}</p>");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send booking created email.");
        }
    }

    private async Task TrySendBookingStatusEmail(int bookingId)
    {
        try
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .Include(x => x.Property)
                .Include(x => x.Renter)
                .FirstOrDefaultAsync(x => x.Id == bookingId);

            if (booking == null || string.IsNullOrWhiteSpace(booking.Renter?.Email))
                return;

            await _emailService.SendAsync(
                booking.Renter.Email,
                $"Booking status updated: {booking.Status}",
                $"<p>Your booking for <strong>{booking.Property?.Title}</strong> is now <strong>{booking.Status}</strong>.</p>");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send booking status email.");
        }
    }
}
