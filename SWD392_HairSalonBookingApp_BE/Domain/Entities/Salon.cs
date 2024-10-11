namespace Domain.Entities
{
    public class Salon : BaseEntity
    {
        public string Address { get; set; }

        #region RelationShip
        public ICollection<ComboService> ComboServices { get; set; }
        public ICollection<SalonMember> SalonMembers { get; set; }
        #endregion
    }
}
