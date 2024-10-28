namespace Domain.Entities
{
    public class SalonMember : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Job { get; set; }
        public Guid UserId { get; set; }
        public string? Rating { get; set; }

        #region RelationShip
        public ICollection<SalonMemberSchedule> SalonMemberSchedules { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public User User { get; set; }
        public Guid SalonId { get; set; }
        public Salon Salon { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        #endregion
    }
}
