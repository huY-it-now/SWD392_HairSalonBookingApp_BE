namespace Domain.Entities
{
    public class Booking : BaseEntity
    {
        public string? Feedback { get; set; }
        public decimal TotalMoney { get; set; }

        #region Relationship
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid SalonId { get; set; }
        public Salon Salon { get; set; }
        #endregion
    }
}
