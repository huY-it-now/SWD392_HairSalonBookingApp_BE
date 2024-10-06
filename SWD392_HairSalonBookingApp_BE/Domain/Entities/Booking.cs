namespace Domain.Entities
{
    public class Booking : BaseEntity
    {
        public string? Feedback { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalMoney { get; set; }

        #region Relationship
        public ICollection<Payments> Payments { get; set; }
        public ICollection<BookingDetails> BookingDetails { get; set; }
        public ICollection<SalonMember> SalonMembers { get; set; }
        #endregion
    }
}
