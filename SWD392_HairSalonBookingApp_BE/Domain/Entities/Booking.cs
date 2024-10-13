namespace Domain.Entities
{
    public class Booking : BaseEntity
    {
        public string? Feedback { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalMoney { get; set; }
        public bool Checked { get; set; }

        #region Relationship
        public ICollection<Payments> Payments { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid SalonId { get; set; }
        public Salon salon { get; set; }
        public Guid SalonMemberId { get; set; }
        public SalonMember SalonMember { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public ICollection<ComboService> ComboServices { get; set; }
        #endregion
    }
}
