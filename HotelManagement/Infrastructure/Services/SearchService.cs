using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;

    public SearchService( IPropertyRepository propertyRepository, IRoomTypeRepository roomTypeRepository, IReservationRepository reservationRepository )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<IEnumerable<SearchResultItem>> SearchAvailableAsync( string? city, DateOnly arrivalDate, DateOnly departureDate, int guests, decimal? maxPrice )
    {
        if ( arrivalDate >= departureDate )
        {
            throw new ArgumentException( "Arrival date must be before departure date." );
        }

        var allProperties = await _propertyRepository.GetAllAsync();
        if ( !string.IsNullOrEmpty( city ) )
        {
            allProperties = allProperties.Where( p => p.City.Equals( city, StringComparison.OrdinalIgnoreCase ) );
        }

        var results = new List<SearchResultItem>();
        foreach ( var property in allProperties )
        {
            var roomTypes = await _roomTypeRepository.GetByPropertyIdAsync( property.Id );
            foreach ( var rt in roomTypes )
            {
                // Фильтрация по кол-ву гостей
                if ( guests < rt.MinPersonCount || guests > rt.MaxPersonCount )
                {
                    continue;
                }

                // Фильтрация по цене
                if ( maxPrice.HasValue && rt.DailyPrice > maxPrice.Value )
                {
                    continue;
                }

                // Доступность
                var overlapping = await _reservationRepository.GetOverlappingReservationsCountAsync( rt.Id, arrivalDate, departureDate );
                if ( overlapping >= rt.AvailableRoomsCount )
                {
                    continue;
                }

                // Подсчёт цены
                int nights = departureDate.DayNumber - arrivalDate.DayNumber;
                decimal total = rt.DailyPrice * nights;

                results.Add( new SearchResultItem
                {
                    PropertyId = property.Id,
                    PropertyName = property.Name,
                    City = property.City,
                    RoomTypeId = rt.Id,
                    RoomTypeName = rt.Name,
                    DailyPrice = rt.DailyPrice,
                    Currency = rt.Currency,
                    TotalForStay = total,
                    AvailableRooms = rt.AvailableRoomsCount - overlapping
                } );
            }
        }

        return results;
    }
}