namespace Domain.Contracts.DTO.Booking
{
    public class BookingUserDTO
    {
        public Guid UserId { get; set; }
        public List<BookingDTO> Bookings { get; set; }
    }
}
