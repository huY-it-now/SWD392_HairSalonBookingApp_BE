namespace Domain.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceService { get; set; }

        #region Relationship
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ComboService> ComboServices { get; set; }
        #endregion
    }
}
