namespace Domain.Entities
{
    public class ComboService : BaseEntity
    {
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }

        #region Relationship
        public ICollection<Service> Service { get; set; }
        public ICollection<BookingDetails> BookingDetails { get; set; }
        public ICollection<ComboDetail> ComboDetails { get; set; }
        #endregion
    }
}
