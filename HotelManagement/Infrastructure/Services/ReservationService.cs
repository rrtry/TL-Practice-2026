using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(
        IReservationRepository reservationRepository,
        IRoomTypeRepository roomTypeRepository,
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork )
    {
        _reservationRepository = reservationRepository;
        _roomTypeRepository = roomTypeRepository;
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Reservation> CreateReservationAsync( Reservation reservation )
    {
        // Дата заселения должна предшествовать дате отбытия
        ValidateDateAndTime( reservation );

        // Существует ли недвижимость
        await ValidatePropertyExistsAsync( reservation.PropertyId );

        // Существует ли тип номера
        var roomType = await ValidateAndGetRoomTypeAsync( reservation.RoomTypeId, reservation.PropertyId );

        // Проверка кол-ва гостей
        ValidateGuestCount( reservation.GuestCount, roomType );

        // Проверка доступности номеров в указанный период.
        await ValidateAvailabilityAsync( roomType.Id, reservation.ArrivalDate, reservation.DepartureDate, roomType.AvailableRoomsCount );

        // Цена
        int nights = reservation.DepartureDate.DayNumber - reservation.ArrivalDate.DayNumber;
        decimal dailyPrice = roomType.DailyPrice;

        reservation.Total = dailyPrice * nights;
        reservation.Currency = roomType.Currency;

        await _reservationRepository.AddAsync( reservation );
        await _unitOfWork.SaveChangesAsync();

        return reservation;
    }

    public async Task<IEnumerable<Reservation>> GetFilteredReservationsAsync( ReservationFilter filter )
    {
        return await _reservationRepository.GetFilteredAsync( filter );
    }

    public async Task<Reservation> GetReservationByIdAsync( Guid id )
    {
        Reservation? reservation = await _reservationRepository.GetByIdAsync( id );
        if ( reservation == null )
        {
            throw new ReservationNotFoundException( id );
        }

        return reservation;
    }

    public async Task DeleteReservationAsync( Guid id )
    {
        Reservation? reservation = await _reservationRepository.GetByIdAsyncForUpdate( id );
        if ( reservation == null )
        {
            throw new ReservationNotFoundException( id );
        }

        _reservationRepository.Delete( reservation );
        await _unitOfWork.SaveChangesAsync();
    }

    private void ValidateDateAndTime( Reservation reservation )
    {
        if ( reservation.ArrivalDate >= reservation.DepartureDate )
        {
            throw new InvalidDateRangeException( reservation.ArrivalDate, reservation.DepartureDate );
        }

        var now = DateTime.UtcNow;
        var nowDate = DateOnly.FromDateTime( now );
        var nowTime = TimeOnly.FromDateTime( now );

        if ( reservation.ArrivalDate < nowDate )
        {
            throw new InvalidArrivalDateException( reservation.ArrivalDate, nowDate );
        }

        if ( reservation.ArrivalDate == nowDate && reservation.ArrivalTime >= nowTime )
        {
            throw new InvalidArrivalTimeException( reservation.ArrivalTime, nowTime );
        }
    }

    private async Task ValidatePropertyExistsAsync( Guid propertyId )
    {
        if ( !await _propertyRepository.ExistsAsync( propertyId ) )
        {
            throw new PropertyNotFoundException( propertyId );
        }
    }

    private async Task<RoomType> ValidateAndGetRoomTypeAsync( Guid roomTypeId, Guid propertyId )
    {
        RoomType? roomType = await _roomTypeRepository.GetByIdAsync( roomTypeId );
        if ( roomType == null )
        {
            throw new RoomTypeNotFoundException( roomTypeId );
        }

        if ( roomType.PropertyId != propertyId )
        {
            throw new RoomTypeMismatchException( roomTypeId, propertyId, roomType.PropertyId );
        }

        return roomType;
    }

    private void ValidateGuestCount( int guestCount, RoomType roomType )
    {
        if ( guestCount < roomType.MinPersonCount || guestCount > roomType.MaxPersonCount )
        {
            throw new GuestCountOutOfRangeException( roomType.MinPersonCount, roomType.MaxPersonCount, guestCount );
        }
    }

    private async Task ValidateAvailabilityAsync( Guid roomTypeId, DateOnly arrival, DateOnly departure, int availableRoomsCount )
    {
        int overlapping = await _reservationRepository.GetOverlappingReservationsCountAsync( roomTypeId, arrival, departure );
        if ( overlapping >= availableRoomsCount )
        {
            throw new NoAvailableRoomsException( roomTypeId, arrival, departure );
        }
    }
}