namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        #region Relationship
        public ICollection<Service> Services { get; set; }

        #endregion
    }
}
