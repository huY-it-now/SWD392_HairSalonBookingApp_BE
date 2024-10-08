namespace Domain.Entities
{
    public class SalonMember : BaseEntity
    {
        public string Job { get; set; }
        public Guid UserId { get; set; }
        public string? Rating { get; set; }

        #region RelationShip
        public User User { get; set; }
        public ICollection<SalonWithMembers> SalonWithMembers { get; set; }
        #endregion
    }
}
