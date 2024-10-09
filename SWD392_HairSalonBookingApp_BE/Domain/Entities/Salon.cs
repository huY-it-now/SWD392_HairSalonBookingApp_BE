namespace Domain.Entities
{
    public class Salon : BaseEntity
    {
        public string Address { get; set; }

        #region RelationShip
        public ICollection<Category> Categories { get; set; }
        public ICollection<SalonMember> SalonMembers { get; set; }
        #endregion
    }
}
