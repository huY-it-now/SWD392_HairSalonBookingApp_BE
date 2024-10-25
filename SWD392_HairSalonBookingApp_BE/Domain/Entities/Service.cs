namespace Domain.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceName { get; set; }
        #region Relationship
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ServiceComboService> ServiceComboServices { get; set; }
        
        public ICollection<Booking> Bookings { get; set; }
        #endregion
    }
}
