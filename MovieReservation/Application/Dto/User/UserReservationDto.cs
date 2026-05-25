using Application.Dto.Reservation;

namespace Application.Dto.User;

public class UserReservationDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<ReservationDto> Reservations { get; set; } = [];
}