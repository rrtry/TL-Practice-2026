using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public static class DbInitializer
{
    public static async Task SeedAsync( HotelManagementDbContext context )
    {
        await context.Database.EnsureCreatedAsync();

        if ( await context.Properties.AnyAsync() )
        {
            return;
        }

        // Тестовые данные: Отели/Недвижимость
        var propertyMoscow = new Property
        {
            Id = Guid.NewGuid(),
            Name = "Гранд Отель Москва",
            Country = "RUS",
            City = "Москва",
            Address = "Тверская ул., 1",
            Latitude = 55.7558,
            Longitude = 37.6173
        };

        var propertySochi = new Property
        {
            Id = Guid.NewGuid(),
            Name = "Приморский Резорт",
            Country = "RUS",
            City = "Сочи",
            Address = "Набережная, 5",
            Latitude = 43.5855,
            Longitude = 39.7231
        };

        var propertySpb = new Property
        {
            Id = Guid.NewGuid(),
            Name = "Невский Палас",
            Country = "RUS",
            City = "Санкт-Петербург",
            Address = "Невский просп., 100",
            Latitude = 59.9343,
            Longitude = 30.3351
        };

        var propertyKazan = new Property
        {
            Id = Guid.NewGuid(),
            Name = "Казанский Дворик",
            Country = "RUS",
            City = "Казань",
            Address = "ул. Баумана, 10",
            Latitude = 55.7964,
            Longitude = 49.1089
        };

        context.Properties.AddRange( propertyMoscow, propertySochi, propertySpb, propertyKazan );
        await context.SaveChangesAsync();

        // Тестовые данные: типы номеров
        var rtMoscowStandard = CreateRoomType(
            propertyMoscow.Id, "Стандарт", 4500, "RUB", 1, 2, 15,
            new() { "Wi-Fi", "Телевизор" },
            new() { "Душ", "Кондиционер" } );

        var rtMoscowSuperior = CreateRoomType(
            propertyMoscow.Id, "Улучшенный", 7500, "RUB", 1, 3, 10,
            new() { "Wi-Fi", "Телевизор", "Халат" },
            new() { "Ванна", "Мини-бар", "Сейф" } );

        var rtMoscowLux = CreateRoomType(
            propertyMoscow.Id, "Люкс", 15000, "RUB", 1, 4, 5,
            new() { "Wi-Fi", "Телевизор", "Персональный дворецкий" },
            new() { "Джакузи", "Гостиная", "Панорамный вид" } );

        // Сочи
        var rtSochiStandard = CreateRoomType(
            propertySochi.Id, "Стандарт с видом на море", 6000, "RUB", 1, 2, 20,
            new() { "Wi-Fi", "Телевизор", "Шезлонги" },
            new() { "Душ", "Балкон" } );

        var rtSochiSuite = CreateRoomType(
            propertySochi.Id, "Сьют", 12000, "RUB", 2, 4, 8,
            new() { "Wi-Fi", "Телевизор", "Бассейн", "Спа" },
            new() { "Ванна с гидромассажем", "Терраса", "Кофемашина" } );

        // Санкт-Петербург
        var rtSpbStandard = CreateRoomType(
            propertySpb.Id, "Классик", 5000, "RUB", 1, 2, 12,
            new() { "Wi-Fi", "Кабельное ТВ" },
            new() { "Душ", "Отопление" } );

        var rtSpbDeluxe = CreateRoomType(
            propertySpb.Id, "Делюкс", 9000, "RUB", 1, 3, 6,
            new() { "Wi-Fi", "Телевизор", "Пресса" },
            new() { "Ванна", "Гардеробная", "Утюг" } );

        // Казань
        var rtKazanStandard = CreateRoomType(
            propertyKazan.Id, "Эконом", 3000, "RUB", 1, 2, 18,
            new() { "Wi-Fi" },
            new() { "Душ", "Вентилятор" } );

        var rtKazanComfort = CreateRoomType(
            propertyKazan.Id, "Комфорт", 5500, "RUB", 2, 4, 10,
            new() { "Wi-Fi", "Телевизор", "Чайник" },
            new() { "Ванна", "Кондиционер", "Холодильник" } );

        context.RoomTypes.AddRange(
            rtMoscowStandard, rtMoscowSuperior, rtMoscowLux,
            rtSochiStandard, rtSochiSuite,
            rtSpbStandard, rtSpbDeluxe,
            rtKazanStandard, rtKazanComfort );
        await context.SaveChangesAsync();

        // Тестовые данные бронирования
        var today = DateOnly.FromDateTime( DateTime.Today );
        var reservations = new List<Reservation>
        {
            CreateReservation(propertyMoscow.Id, rtMoscowStandard.Id, today.AddDays(1), today.AddDays(3), 2,
                "Иван Петров", "+79161234567", rtMoscowStandard.DailyPrice, rtMoscowStandard.Currency),

            CreateReservation(propertyMoscow.Id, rtMoscowStandard.Id, today.AddDays(2), today.AddDays(5), 1,
                "Мария Смирнова", "+79161234568", rtMoscowStandard.DailyPrice, rtMoscowStandard.Currency),

            CreateReservation(propertyMoscow.Id, rtMoscowSuperior.Id, today.AddDays(3), today.AddDays(7), 3,
                "Алексей Орлов", "+79161234569", rtMoscowSuperior.DailyPrice, rtMoscowSuperior.Currency),

            CreateReservation(propertySochi.Id, rtSochiSuite.Id, today.AddDays(5), today.AddDays(10), 2,
                "Ольга Кузнецова", "+79881234567", rtSochiSuite.DailyPrice, rtSochiSuite.Currency),

            CreateReservation(propertySochi.Id, rtSochiSuite.Id, today.AddDays(7), today.AddDays(12), 3,
                "Дмитрий Васильев", "+79881234568", rtSochiSuite.DailyPrice, rtSochiSuite.Currency),

            CreateReservation(propertySpb.Id, rtSpbStandard.Id, today.AddDays(2), today.AddDays(6), 1,
                "Екатерина Михайлова", "+79211234567", rtSpbStandard.DailyPrice, rtSpbStandard.Currency),

            CreateReservation(propertyKazan.Id, rtKazanComfort.Id, today.AddDays(4), today.AddDays(8), 4,
                "Рустем Галиев", "+78431234567", rtKazanComfort.DailyPrice, rtKazanComfort.Currency),

            CreateReservation(propertyKazan.Id, rtKazanComfort.Id, today.AddDays(6), today.AddDays(9), 2,
                "Алсу Закирова", "+78431234568", rtKazanComfort.DailyPrice, rtKazanComfort.Currency)
        };

        context.Reservations.AddRange( reservations );
        await context.SaveChangesAsync();
    }

    private static RoomType CreateRoomType(
        Guid propertyId, string name, decimal dailyPrice, string currency,
        int minPerson, int maxPerson, int availableRooms,
        List<string> services, List<string> amenities )
    {
        return new RoomType
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            Name = name,
            DailyPrice = dailyPrice,
            Currency = currency,
            MinPersonCount = minPerson,
            MaxPersonCount = maxPerson,
            AvailableRoomsCount = availableRooms,
            Services = services,
            Amenities = amenities
        };
    }

    private static Reservation CreateReservation(
        Guid propertyId, Guid roomTypeId, DateOnly arrival, DateOnly departure,
        int guestCount, string guestName, string guestPhone,
        decimal dailyPrice, string currency )
    {
        int nights = departure.DayNumber - arrival.DayNumber;
        return new Reservation
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            RoomTypeId = roomTypeId,
            ArrivalDate = arrival,
            DepartureDate = departure,
            ArrivalTime = new TimeOnly( 14, 0 ),
            DepartureTime = new TimeOnly( 12, 0 ),
            GuestName = guestName,
            GuestPhoneNumber = guestPhone,
            GuestCount = guestCount,
            Total = dailyPrice * nights,
            Currency = currency
        };
    }
}