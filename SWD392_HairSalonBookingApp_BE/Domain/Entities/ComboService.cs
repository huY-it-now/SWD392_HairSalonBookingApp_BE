namespace Domain.Entities
{
    public class ComboService : BaseEntity
    {
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string? ImageId { get; set; }

        #region Relationship
        public ICollection<Service> Service { get; set; }
        public ICollection<ComboServiceComboDetail> ComboServiceComboDetails { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Guid SalonId { get; set; }
        public Salon Salon { get; set; }
        #endregion
    }
}
