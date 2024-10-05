namespace Domain.Entities
{
    public class ComboService : BaseEntity
    {
        public string ComboServiceName { get; set; }
        public decimal Money { get; set; }

        #region Relationship
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public ICollection<ComboDetail> ComboDetails { get; set; }
        #endregion
    }
}
