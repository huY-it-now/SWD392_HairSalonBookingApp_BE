namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        #region Relationship
        public Guid SalonId { get; set; }
        public Salon Salon { get; set; }

        public ICollection<Service> Services { get; set; }
        #endregion
    }
}
