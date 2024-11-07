namespace Domain.Entities
{
    public class ComboDetail : BaseEntity
    {
        public string? Content { get; set; }

        #region Relationship
        public ICollection<ComboServiceComboDetail> ComboServiceComboDetails { get; set; }
        #endregion
    }
}
