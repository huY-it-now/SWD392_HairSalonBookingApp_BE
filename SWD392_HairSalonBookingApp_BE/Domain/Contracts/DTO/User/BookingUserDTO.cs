using Domain.Contracts.DTO.Booking;

namespace Domain.Contracts.DTO.User
{
    public class BookingUserDTO
    {
        public Guid UserId { get; set; }
        public List<BookingDTO> Bookings { get; set; }
    }
}
