using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services;

public interface IBookingService
{
    Task<(bool Success, string Message, BookingResponseDto? Booking)> CreateAsync(BookingCreateDto dto, int renterId);
    Task<IReadOnlyList<BookingResponseDto>> GetRenterBookingsAsync(int renterId);
    Task<IReadOnlyList<BookingResponseDto>> GetOwnerBookingsAsync(int ownerId);
    Task<(bool Success, string Message)> CancelByRenterAsync(int bookingId, int renterId);
    Task<(bool Success, string Message)> UpdateStatusByOwnerAsync(int bookingId, int ownerId, string status);
}
