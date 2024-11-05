namespace Domain.Entities
{
    public class Booking : BaseEntity
    {
        public string? Feedback { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }

        #region Relationship
        public Guid PaymentId { get; set; }
        public Payments Payments { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid SalonId { get; set; }
        public Salon salon { get; set; }
        public Guid SalonMemberId { get; set; }
        public SalonMember SalonMember { get; set; }
        public Guid ComboServiceId { get; set; }
        public ComboService ComboService { get; set; }
        #endregion
    }
}
