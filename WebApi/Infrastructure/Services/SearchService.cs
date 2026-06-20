using Domain.Exceptions;
using Domain.Filters;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;

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

    public async Task<IEnumerable<SearchResultItem>> SearchAvailableAsync( SearchFilter filter )
    {
        if ( filter.ArrivalDate >= filter.DepartureDate )
        {
            throw new InvalidDateRangeException( filter.ArrivalDate, filter.DepartureDate );
        }

        var properties = await _propertyRepository.GetByCityAsync( filter.City );
        var results = new List<SearchResultItem>();

        foreach ( var property in properties )
        {
            var roomTypes = await _roomTypeRepository.GetByPropertyIdAsync( property.Id );
            foreach ( var rt in roomTypes )
            {
                // Фильтрация по кол-ву гостей
                if ( filter.Guests < rt.MinPersonCount || filter.Guests > rt.MaxPersonCount )
                {
                    continue;
                }

                // Фильтрация по цене
                if ( filter.MaxPrice.HasValue && rt.DailyPrice > filter.MaxPrice.Value )
                {
                    continue;
                }

                // Доступность
                int overlapping = await _reservationRepository.GetOverlappingReservationsCountAsync( rt.Id, filter.ArrivalDate, filter.DepartureDate );

                if ( overlapping >= rt.AvailableRoomsCount )
                {
                    continue;
                }

                // Подсчёт цены
                int nights = filter.DepartureDate.DayNumber - filter.ArrivalDate.DayNumber;
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