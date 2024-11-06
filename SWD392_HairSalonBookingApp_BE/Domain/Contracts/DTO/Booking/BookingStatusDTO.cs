using Domain.Contracts.DTO.Combo;

namespace Domain.Contracts.DTO.Booking
{
    public class BookingStatusDTO
    {
        public Guid BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public Guid StylistId { get; set; }
        public string StylistName { get; set; }
        public ComboServiceForBookingDTO ComboServiceName { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
