namespace Domain.Entities
{
    public class ComboService : BaseEntity
    {
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }

        #region Relationship
        public ICollection<Service> Service { get; set; }
        public ICollection<ComboDetail> ComboDetails { get; set; }
        public Guid SalonId { get; set; }
        public Salon Salon { get; set; }
        #endregion
    }
}
