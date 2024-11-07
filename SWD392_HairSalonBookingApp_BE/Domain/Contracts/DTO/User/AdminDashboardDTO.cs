using Domain.Contracts.DTO.Booking;

namespace Domain.Contracts.DTO.User
{
    public class AdminDashboardDTO
    {
        public int TotalBookings { get; set; }
        public List<BookingDTO> Bookings { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
