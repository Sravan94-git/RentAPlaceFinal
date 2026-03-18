using RentAPlace.API.Data;
using RentAPlace.API.Models;
using BCrypt.Net;

namespace RentAPlace.API.Extensions;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // ── Seed demo OWNER ──────────────────────────────────────────────────
        var owner = context.Users.FirstOrDefault(u => u.Email == "owner@demo.com");
        if (owner == null)
        {
            owner = new User
            {
                FullName = "Demo Owner",
                Email = "owner@demo.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = "Owner"
            };
            context.Users.Add(owner);
            context.SaveChanges();
        }

        // ── Seed demo RENTER ──────────────────────────────────────────────────
        var renter = context.Users.FirstOrDefault(u => u.Email == "renter@demo.com");
        if (renter == null)
        {
            renter = new User
            {
                FullName = "Demo Renter",
                Email = "renter@demo.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = "Renter"
            };
            context.Users.Add(renter);
            context.SaveChanges();
        }

        // ── Seed PROPERTIES (Only if table is empty) ─────────────────────
        if (!context.Properties.Any())
        {
            var properties = new List<Property>
            {
                // ── 4 Villas ──────────────────────────────────────────────────────
                new Property {
                    Title = "Oceanfront Estate", Location = "Maldives", PropertyType = "Villa",
                    Price = 45000, MaxGuests = 10, HasPool = true, IsBeachFacing = true, HasGarden = true, Rating = 4.9m,
                    ImageUrl = "https://boulevard.co/wp-content/uploads/2023/11/soneva-fushi-beach-villa-pool-5.png", // ID: 53
                    Description = "A breathtaking overwater villa perched above the crystal-clear lagoons of the Maldives. Wake up to panoramic ocean views from your private deck, take a dip in the infinity pool that merges with the sea, or snorkel directly from your villa steps. The interior features hand-picked coral-stone accents, king-sized beds with linen sheets, and a personal butler on call 24/7. Perfect for a once-in-a-lifetime escape.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Tuscan Vineyard Retreat", Location = "Italy", PropertyType = "Villa",
                    Price = 20000, MaxGuests = 8, HasPool = true, IsBeachFacing = false, HasGarden = true, Rating = 4.7m,
                    ImageUrl = "https://images.stockcake.com/public/0/1/8/0184f5a5-201c-4bf3-9145-e806f838eced_large/idyllic-tuscan-villa-stockcake.jpg", // ID: 54
                    Description = "Nestled among rolling vineyards in the heart of Tuscany, this stone villa dating from the 18th century has been lovingly restored with modern comforts. Sip estate-grown Chianti on the terrace at sunset, cook Tuscan feasts in the fully-equipped farmhouse kitchen, or explore the medieval hilltop villages minutes away. The garden pool overlooks rows of grapevines stretching to the horizon.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Bali Jungle Sanctuary", Location = "Bali", PropertyType = "Villa",
                    Price = 18500, MaxGuests = 6, HasPool = true, IsBeachFacing = false, HasGarden = true, Rating = 4.8m,
                    ImageUrl = "https://bali-home-immo.com/images/properties/jungle-view-5-bedroom-villa-for-sale-leasehold-in-bali-ubud-rf37217gQsrxhbIE90WR6p6F651713947934.jpg", // ID: 55
                    Description = "Immerse yourself in the lush greenery of Ubud's jungle canopy at this award-winning villa. Traditional Balinese architecture blends seamlessly with open-air living spaces, a private plunge pool surrounded by tropical foliage, and a meditation pavilion overlooking a river gorge. Enjoy complimentary daily breakfast, in-villa massages, and guided temple tours arranged by your dedicated host.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Modern Glass Villa", Location = "California", PropertyType = "Villa",
                    Price = 32000, MaxGuests = 12, HasPool = true, IsBeachFacing = true, HasGarden = false, Rating = 4.4m,
                    ImageUrl = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&q=80", // Kept original Unsplash URL
                    Description = "An architectural masterpiece perched on the Malibu cliffs with floor-to-ceiling glass walls framing the Pacific Ocean on three sides. This contemporary villa features an open-plan entertainment space, a chef's kitchen with Sub-Zero appliances, a heated infinity pool, and a home cinema. Ideal for large groups, celebrations, or creative retreats seeking the ultimate California luxury experience.",
                    OwnerId = owner.Id
                },

                // ── 6 Apartments ─────────────────────────────────────────────────
                new Property {
                    Title = "Skyline Penthouse", Location = "New York", PropertyType = "Apartment",
                    Price = 25000, MaxGuests = 4, HasPool = false, IsBeachFacing = false, HasGarden = false, Rating = 4.9m,
                    ImageUrl = "https://media.admiddleeast.com/photos/6492ca07a8abf4a96208d046/16:9/w_2560%2Cc_limit/1025-Simone%2520Bossi%2520%25C2%25A9%25202022.jpg", // ID: 57
                    Description = "Occupy the crown of a Midtown Manhattan skyscraper with unobstructed 360-degree views of Central Park, the Hudson River, and the glittering city skyline. This full-floor penthouse features soaring 4-metre ceilings, polished marble floors, a wraparound terrace, and bespoke Italian furniture throughout. Door-to-door car service, a private chef, and curated NYC itineraries are included with every stay.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Downtown Loft", Location = "London", PropertyType = "Apartment",
                    Price = 12000, MaxGuests = 2, HasPool = false, IsBeachFacing = false, HasGarden = false, Rating = 4.5m,
                    ImageUrl = "https://www.domino.com/wp-content/uploads/2025/12/Duelle-London-Loft-Home-Tour-Domino7.jpg?strip=all&quality=85&w=1620", // ID: 58
                    Description = "A converted Victorian warehouse loft in the heart of Shoreditch, London's most creative neighbourhood. Original exposed brick walls, salvaged timber beams, and industrial steel windows meet bespoke designer furniture and the latest smart-home technology. Walk to world-class galleries, street-food markets, and the city's best cocktail bars. The Elizabeth line stop is just 3 minutes on foot.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Seine River View", Location = "Paris", PropertyType = "Apartment",
                    Price = 18000, MaxGuests = 3, HasPool = false, IsBeachFacing = false, HasGarden = false, Rating = 4.8m,
                    ImageUrl = "https://www.guestapartment.com/wp-content/uploads/2016/08/Paris-river-seine-view-balcony-790x525.jpg", // ID: 59
                    Description = "A Haussmann-era apartment on the Île Saint-Louis with a wrap-around juliet balcony overlooking Notre-Dame and the Seine. Decorated with original 19th-century parquet floors, ornate cornicing, and a curated collection of French antiques, yet fully modernised with a designer kitchen and rain-head bathrooms. The Marais, Saint-Germain, and the Louvre are all within a 10-minute stroll.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Marina Bay Studio", Location = "Singapore", PropertyType = "Apartment",
                    Price = 9500, MaxGuests = 2, HasPool = true, IsBeachFacing = false, HasGarden = true, Rating = 4.6m,
                    ImageUrl = "https://dam.mediacorp.sg/image/upload/s--UDdRJzYD--/c_crop,h_743,w_1322,x_4,y_757/c_fill,g_auto,h_676,w_1200/f_auto,q_auto/v1/mediacorp/cna/image/2022/04/27/super_penthouse_marina_bay_residences_studio_if.png?itok=p3oZw8sc", // ID: 60
                    Description = "Stay in one of Singapore's most iconic addresses with Marina Bay Sands and the glittering city skyline as your backdrop. This sleek studio apartment on the 38th floor features floor-to-ceiling windows, a queen-size platform bed, and access to an infinity rooftop pool. The MRT is steps away, giving you instant access to Gardens by the Bay, Orchard Road, and the hawker centres beloved by food lovers.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Tokyo Tech Hub Pad", Location = "Tokyo", PropertyType = "Apartment",
                    Price = 11000, MaxGuests = 2, HasPool = false, IsBeachFacing = false, HasGarden = false, Rating = 4.3m,
                    ImageUrl = "https://i.ytimg.com/vi/lBnLdqfsq18/hq720.jpg?sqp=-oaymwEhCK4FEIIDSFryq4qpAxMIARUAAAAAGAElAADIQj0AgKJD&rs=AOn4CLDvzKr-OuOMWZtx55xsNWT9l9vYhQ", // ID: 61
                    Description = "A minimalist, precision-engineered apartment in Shinjuku, designed around the principles of Japanese wabi-sabi. Clean lines, warm timber panels, handmade ceramics, and a state-of-the-art smart-home system create a serene urban retreat. The building features a shared rooftop garden with Mount Fuji views on clear days. Akihabara, Harajuku, and Shibuya are all within 20 minutes by metro.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Cozy Munich Flat", Location = "Munich", PropertyType = "Apartment",
                    Price = 8000, MaxGuests = 2, HasPool = false, IsBeachFacing = false, HasGarden = false, Rating = 4.2m,
                    ImageUrl = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800&q=80", // Kept original Unsplash URL
                    Description = "A warm and inviting Altbau apartment in Munich's beloved Schwabing district, just two stops from Marienplatz on the U-Bahn. Period features like stucco ceilings and tall sash windows sit alongside a modern fitted kitchen, underfloor heating, and a collection of local art. Perfect for exploring the English Garden, Nymphenburg Palace, the BMW Museum, and the legendary Hofbräuhaus.",
                    OwnerId = owner.Id
                },

                // ── 3 Bungalows ──────────────────────────────────────────────────
                new Property {
                    Title = "Kerala Backwaters Bungalow", Location = "India", PropertyType = "Bungalow",
                    Price = 6000, MaxGuests = 6, HasPool = false, IsBeachFacing = false, HasGarden = true, Rating = 4.7m,
                    ImageUrl = "https://hoppingmiles.com/wp-content/uploads/2017/11/backwaters-2075738_1280-1.jpg", // ID: 63
                    Description = "A traditional Kerala tharavadu (heritage home) set on stilts at the water's edge of Vembanad Lake. Watch the iconic Chinese fishing nets at dawn, glide through the backwaters on a private kettuvallam houseboat excursion, or unwind with an authentic Ayurvedic massage in the garden. The tariff includes home-cooked Kerala seafood meals prepared fresh by your resident cook each morning and evening.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Tropical Bamboo House", Location = "Costa Rica", PropertyType = "Bungalow",
                    Price = 7500, MaxGuests = 4, HasPool = false, IsBeachFacing = true, HasGarden = true, Rating = 4.8m,
                    ImageUrl = "https://costaricarealestate.net/wp-content/uploads/2023/04/Dan-Lancinger-1.jpg", // ID: 64
                    Description = "An eco-architect's dream, this handcrafted bamboo bungalow sits within a private nature reserve on Costa Rica's Nicoya Peninsula. Fall asleep to the sound of howler monkeys and wake to Pacific sunrises from your beachfront deck. The property runs entirely on solar power, harvests rainwater, and serves an organic garden-to-table breakfast daily. Sea-turtle nesting beaches and Pipa surf breaks await just outside your door.",
                    OwnerId = owner.Id
                },
                new Property {
                    Title = "Rainforest Eco Cabin", Location = "Brazil", PropertyType = "Bungalow",
                    Price = 5000, MaxGuests = 2, HasPool = false, IsBeachFacing = false, HasGarden = true, Rating = 4.9m,
                    ImageUrl = "https://www.ampersandtravel.com/media/850663/rainforest-eco-lodge-sinharaja-sri-lanka-2.jpeg?mode=crop", // ID: 65
                    Description = "Hidden deep within the Atlantic Forest of Brazil's Serra Gaúcha wine country, this intimate eco-cabin is the ultimate off-grid escape. The cabin is built from reclaimed timber with a living-grass roof, solar electricity, and a wood-fired hot tub overlooking the forest canopy. Guided waterfall hikes, birdwatching tours, and visits to boutique wineries can be arranged by your host. Mobile signal is intentionally absent — pure digital detox.",
                    OwnerId = owner.Id
                }
            };

            context.Properties.AddRange(properties);
            context.SaveChanges();
        }

        // ── Seed MOCK BOOKINGS for the demo renter ────────────────────────────
        if (!context.Bookings.Any() && context.Properties.Any())
        {
            var villa = context.Properties.FirstOrDefault(p => p.Title == "Bali Jungle Sanctuary");
            var apt   = context.Properties.FirstOrDefault(p => p.Title == "Skyline Penthouse");
            var bung  = context.Properties.FirstOrDefault(p => p.Title == "Tropical Bamboo House");

            // Ensure properties exist before creating bookings
            if (villa != null && apt != null && bung != null)
            {
                var bookings = new List<Booking>
                {
                    new Booking
                    {
                        PropertyId   = villa.Id,
                        RenterId     = renter.Id,
                        CheckInDate  = new DateTime(2026, 4, 10),
                        CheckOutDate = new DateTime(2026, 4, 17),
                        Guests       = 4,
                        TotalPrice   = villa.Price * 7,
                        Status       = "Confirmed",
                        CreatedAt    = DateTime.UtcNow
                    },
                    new Booking
                    {
                        PropertyId   = apt.Id,
                        RenterId     = renter.Id,
                        CheckInDate  = new DateTime(2026, 5, 1),
                        CheckOutDate = new DateTime(2026, 5, 5),
                        Guests       = 2,
                        TotalPrice   = apt.Price * 4,
                        Status       = "Pending",
                        CreatedAt    = DateTime.UtcNow
                    },
                    new Booking
                    {
                        PropertyId   = bung.Id,
                        RenterId     = renter.Id,
                        CheckInDate  = new DateTime(2026, 3, 1),
                        CheckOutDate = new DateTime(2026, 3, 5),
                        Guests       = 2,
                        TotalPrice   = bung.Price * 4,
                        Status       = "Cancelled",
                        CreatedAt    = DateTime.UtcNow
                    }
                };

                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }
        }
    }
}