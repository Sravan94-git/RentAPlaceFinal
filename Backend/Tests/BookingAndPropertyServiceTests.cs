using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using RentAPlace.API.Models;
using RentAPlace.API.Services;

namespace RentAPlace.Tests;

public class BookingAndPropertyServiceTests
{
    [Fact]
    public async Task CreateBooking_ShouldFail_WhenCheckoutIsBeforeCheckin()
    {
        await using var context = CreateDbContext();
        SeedUsersAndProperty(context);

        var service = new BookingService(context, CreateEmailService(), NullLogger<BookingService>.Instance);

        var dto = new BookingCreateDto
        {
            PropertyId = 1,
            CheckInDate = new DateTime(2026, 4, 10),
            CheckOutDate = new DateTime(2026, 4, 9),
            Guests = 2
        };

        var result = await service.CreateAsync(dto, renterId: 2);

        Assert.False(result.Success);
        Assert.Null(result.Booking);
    }

    [Fact]
    public async Task CreateBooking_ShouldCalculateTotalPrice_ByNights()
    {
        await using var context = CreateDbContext();
        SeedUsersAndProperty(context);

        var service = new BookingService(context, CreateEmailService(), NullLogger<BookingService>.Instance);

        var dto = new BookingCreateDto
        {
            PropertyId = 1,
            CheckInDate = new DateTime(2026, 4, 10),
            CheckOutDate = new DateTime(2026, 4, 13),
            Guests = 2
        };

        var result = await service.CreateAsync(dto, renterId: 2);

        Assert.True(result.Success);
        Assert.NotNull(result.Booking);
        Assert.Equal(3 * 2000m, result.Booking!.TotalPrice);
    }

    [Fact]
    public async Task PropertySearch_ShouldFilterByLocation()
    {
        await using var context = CreateDbContext();
        SeedUsersAndProperty(context);
        context.Properties.Add(new Property
        {
            Id = 2,
            Title = "City Flat",
            Location = "Hyderabad",
            PropertyType = "Flat",
            Price = 1000,
            MaxGuests = 2,
            OwnerId = 1
        });
        await context.SaveChangesAsync();

        var service = new PropertyService(context);

        var result = await service.SearchAsync(new PropertyQueryDto
        {
            Location = "Goa",
            Page = 1,
            PageSize = 10
        });

        Assert.Single(result.Items);
        Assert.Equal("Goa", result.Items[0].Location);
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static void SeedUsersAndProperty(AppDbContext context)
    {
        context.Users.AddRange(
            new User
            {
                Id = 1,
                FullName = "Owner User",
                Email = "owner@test.com",
                Role = "Owner",
                PasswordHash = "x"
            },
            new User
            {
                Id = 2,
                FullName = "Renter User",
                Email = "renter@test.com",
                Role = "Renter",
                PasswordHash = "x"
            });

        context.Properties.Add(new Property
        {
            Id = 1,
            Title = "Beach Villa",
            Location = "Goa",
            PropertyType = "Villa",
            Price = 2000,
            MaxGuests = 4,
            OwnerId = 1
        });

        context.SaveChanges();
    }

    private static IEmailService CreateEmailService()
    {
        return new SmtpEmailService(
            Options.Create(new EmailSettings()),
            NullLogger<SmtpEmailService>.Instance);
    }
}
