using Application.Dto.Event;
using Application.Dto.User;

namespace Application.Dto.Reservation;

public record UserReservationDetailsDto(UserDetailsDto User, ReservationDto ReservationDto,EventDto EventDto);