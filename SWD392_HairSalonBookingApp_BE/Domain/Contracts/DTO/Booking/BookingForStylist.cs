using Domain.Contracts.DTO.Combo;

namespace Domain.Contracts.DTO.Booking
{
    public class BookingForStylist
    {
        public Guid BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingStatus { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public ComboServiceForBookingDTO ComboServiceName { get; set; }
        public string Address { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
