namespace Domain.Entities
{
    public class ComboDetail : BaseEntity
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }

        #region Relationship
        public Guid ComboServiceId { get; set; }
        public ComboService ComboService { get; set; }
        #endregion
    }
}
